using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Conversations.Queries.GetConversationsListByTeamId;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatList;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.TaskItems.Queries.GetTaskItemsList;
using ColabSpace.Application.Teams.Commands.CreateTeam;
using ColabSpace.Application.Teams.Commands.DeleteTeam;
using ColabSpace.Application.Teams.Commands.UpdateTeam;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.Teams.Queries.GetTeam;
using ColabSpace.Application.Teams.Queries.GetTeamsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class TeamController : ApiController
    {
        private readonly IBlobStorageService _blobStorageService;

        public TeamController(IBlobStorageService blobStorageService, ICurrentUserService currentUserService)
        {
            _blobStorageService = blobStorageService;
        }
        [HttpGet("GetAll")]
        public async Task<IEnumerable<TeamModel>> GetAll()
        {
            return await Mediator.Send(new GetTeamsQuery());
        }

        [HttpGet("GetAll/{UserId}")]
        public async Task<IEnumerable<TeamModel>> GetAllByUser(string UserId)
        {
            return await Mediator.Send(new GetTeamsQuery { UserId = UserId });
        }

        [HttpGet("GetAllByTeamName/{TeamName}")]
        public async Task<IEnumerable<TeamModel>> GetAllByTeamName(string TeamName)
        {
            return await Mediator.Send(new GetTeamsQuery { TeamName = TeamName });
        }

        [HttpGet("GetTeam/{TeamId}")]
        public async Task<TeamModel> GetTeamById(string TeamId)
        {
            return await Mediator.Send(new GetTeamQuery { TeamId = new Guid(TeamId) });
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateTeamCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(string id, UpdateTeamCommand command)
        {
            if (!id.Equals(command.Id))
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            IEnumerable<TaskItemModel> tasks = await Mediator.Send(new GetTaskItemsQuery { TeamId = new Guid(id) });
            IEnumerable<ConversationModel> channels = await Mediator.Send(new GetConversationsByTeamIdQuery
            {
                TeamId = new Guid(id),
            });

            // find all message chat
            var messageChats = new List<MessageChatModel>();
            foreach (var channel in channels)
            {
                // find MessageChat async (tim cac MessageChat cung mot luc)
                messageChats.AddRange(await Mediator.Send(new GetMessageChatsQuery { ConversationId = channel.Id }));
            }

            await Mediator.Send(new DeleteTeamCommand { Id = id });

            foreach (TaskItemModel task in tasks)
            {
                // delete file in blob
                if (task.AttachFiles != null)
                {
                    foreach (AttachFileModel file in task.AttachFiles)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
            }

            foreach (MessageChatModel messageChat in messageChats)
            {
                // delete file in blob
                if (messageChat.AttachFileList != null)
                {
                    foreach (AttachFileModel file in messageChat.AttachFileList)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
            }

            return NoContent();
        }
    }
}
