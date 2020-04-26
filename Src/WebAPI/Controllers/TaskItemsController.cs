using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Commands.ChangeStatusTaskItem;
using ColabSpace.Application.TaskItems.Commands.CreateTaskItem;
using ColabSpace.Application.TaskItems.Commands.DeleteTaskItem;
using ColabSpace.Application.TaskItems.Commands.PinTaskItem;
using ColabSpace.Application.TaskItems.Commands.UpdateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList;
using ColabSpace.Application.TaskItems.Queries.GetTaskItem;
using ColabSpace.Application.TaskItems.Queries.GetTaskItemsList;
using ColabSpace.Application.Teams.Queries.GetTeam;
using ColabSpace.WebAPI.Controllers;
using ColabSpace.WebAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class TaskItemsController : ApiController
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IConfiguration _config;

        public TaskItemsController(IBlobStorageService blobStorageService, IHubContext<ChatHub> hubContext, IConfiguration config)
        {
            _blobStorageService = blobStorageService;
            _hubContext = hubContext;
            _config = config;
        }

        [HttpGet("GetAll/{teamId}")]
        public async Task<IEnumerable<TaskItemModel>> SearchTaskInTeam(string teamId, [FromQuery] string keyword, [FromQuery] int? pageIndex)
        {
            return await Mediator.Send(new GetTaskItemsQuery { TeamId = new Guid(teamId), keyword = keyword, pageIndex = pageIndex });
        }

        [HttpGet("GetPin/{teamId}")]
        public async Task<IEnumerable<TaskItemModel>> GetPinnedTaskItemList(Guid teamId, [FromQuery] int? pageIndex)
        {
            return await Mediator.Send(new GetPinnedTaskItemListQuery {
                TeamId = teamId,
                PageIndex = pageIndex
            });
        }

        [HttpGet("GetTaskItem/{taskItemId}")]
        public async Task<TaskItemModel> GetTaskById(string taskItemId)
        {
            return await Mediator.Send(new GetTaskItemQuery { TaskItemId = new Guid(taskItemId) });
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateTaskItemCommand command)
        {
            if (command.AttachFiles != null)
            {
                foreach (AttachFileModel file in command.AttachFiles)
                {
                    file.BlobStorageUrl = await _blobStorageService
                        .UploadFileToBlobAsync(file.LocalUrl.Replace("\\\\", "\\"), file.FileStorageName);
                }
            }
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception)
            {
                if (command.AttachFiles != null)
                {
                    foreach (AttachFileModel file in command.AttachFiles)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
                return BadRequest();
            }
        }

        [HttpPut("ChangeStatus/{id}")]
        public async Task<ActionResult> UpdateStatus(string id, ChangeStatusTaskItemCommand command)
        {
            if (!id.Equals(command.Id.ToString()))
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("Update/{taskId}")]
        public async Task<ActionResult> UpdateTaskItem(string taskId, UpdateTaskItemCommand command)
        {

            if (!taskId.Equals(command.Id.ToString()))
            {
                return BadRequest();
            }

            TaskItemModel task = await Mediator.Send(new GetTaskItemQuery { TaskItemId = new Guid(taskId) });
            if (command.AttachFiles != null)
            {
                // remove file in blob
                List<AttachFileModel> deleteList = task.AttachFiles?.Where(oldFile => !command.AttachFiles.Any(
                    newFile => newFile.FileStorageName.Equals(oldFile.FileStorageName))).ToList();
                if (deleteList != null)
                {
                    foreach (AttachFileModel deleteFile in deleteList)
                    {
                        if (!String.IsNullOrEmpty(deleteFile.BlobStorageUrl))
                        {
                            await _blobStorageService.DeleteBlobData(deleteFile.BlobStorageUrl);
                        }
                    }
                }

                foreach (AttachFileModel file in command.AttachFiles)
                {
                    if (String.IsNullOrEmpty(file.BlobStorageUrl))
                    {
                        file.BlobStorageUrl = await _blobStorageService
                            .UploadFileToBlobAsync(file.LocalUrl.Replace("\\\\", "\\"), file.FileStorageName);
                    }
                }
            }
            else
            {
                // remove file in blob
                if (task.AttachFiles != null)
                {
                    foreach (AttachFileModel deleteFile in task.AttachFiles)
                    {
                        if (!String.IsNullOrEmpty(deleteFile.BlobStorageUrl))
                        {
                            await _blobStorageService.DeleteBlobData(deleteFile.BlobStorageUrl);
                        }
                    }
                }
            }

            try
            {
                await Mediator.Send(command);
            }
            catch (Exception)
            {
                if (command.AttachFiles != null)
                {
                    foreach (AttachFileModel file in command.AttachFiles)
                    {
                        if (String.IsNullOrEmpty(file.BlobStorageUrl))
                        {
                            await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                        }
                    }
                }
            }

            return NoContent();
        }

        [HttpPut("Pin/{taskId}")]
        public async Task<ActionResult> PinTaskItem(Guid taskId, PinTaskItemCommand command)
        {
            if (taskId != command.Id)
            {
                return BadRequest();
            }

            // cap nhat lai task
            var updatedTaskItem = await Mediator.Send(command);

            // lay thong tin team
            var team = await Mediator.Send(new GetTeamQuery
            {
                TeamId = updatedTaskItem.TeamId
            });

            // thong bao cho tat cac cac members cua team
            var lstTaskSendAsync = new List<Task>();
            foreach (var member in team.Users)
            {
                // lay danh dach connection cua user
                foreach (var connectionEntity in GetConnectionEntitys(member.UserId.ToString()))
                {
                    lstTaskSendAsync.Add(_hubContext.Clients.Client(connectionEntity.RowKey).SendAsync("PinTask", updatedTaskItem));
                }
            }
            await Task.WhenAll(lstTaskSendAsync);

            return Ok(updatedTaskItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var task = await Mediator.Send(new DeleteTaskItemCommand { Id = id });

            // delete file in blob
            if (task.AttachFiles != null)
            {
                foreach (AttachFileModel file in task.AttachFiles)
                {
                    await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Lay referce toi mot segment cua table chua tat cac connection cua 1 user
        /// </summary>
        /// <param name="userId">userId duoc luu trong conection table nhu la PartitionKey</param>
        /// <returns></returns>
        private TableQuerySegment<ConnectionEntity> GetConnectionEntitys(string userId)
        {
            var table = GetConnectionTable();
            _ = table.CreateIfNotExistsAsync().Result;

            var query = new TableQuery<ConnectionEntity>()
                .Where(TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                userId));

            return table.ExecuteQuerySegmentedAsync(query, null).Result;
        }

        /// <summary>
        /// lay reference toi connection table luu thong tin ve cac connection cua user
        /// </summary>
        /// <returns></returns>
        private CloudTable GetConnectionTable()
        {
            var storageAccount =
                CloudStorageAccount.Parse(_config["BlobStorage:ConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference("connection");
        }
    }
}