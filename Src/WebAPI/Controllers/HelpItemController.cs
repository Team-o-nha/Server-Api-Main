//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Application.HelpItems.Commands.CreateHelpItem;
//using ColabSpace.Application.HelpItems.Commands.DeleteHelpItem;
//using ColabSpace.Application.HelpItems.Commands.UpdateHelpItem;
//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Application.HelpItems.Queries.GetAllHelpItem;
//using ColabSpace.Application.HelpItems.Queries.GetHelpItemById;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.WindowsAzure.Storage.Blob;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ColabSpace.WebAPI.Controllers
//{
//    [Authorize]
//    public class HelpItemController : ApiController
//    {
//        private readonly IBlobStorageService _blobStorageService;

//        public HelpItemController(IBlobStorageService blobStorageService)
//        {
//            _blobStorageService = blobStorageService;
//        }

//        [HttpPost]
//        public async Task<ActionResult<Guid>> Create(CreateHelpItemCommand command)
//        {
//            try
//            {
//                command.Content.BlobStorageUrl = await _blobStorageService
//                    .UploadFileToBlobAsync(command.Content.LocalUrl.Replace("\\\\", "\\"), command.Content.FileStorageName);
//                return await Mediator.Send(command);
//            }
//            catch (Exception)
//            {
//                // if exception throws and file was already created in blob storage then this file has to be removed
//                if (!string.IsNullOrEmpty(command.Content.BlobStorageUrl))
//                {
//                    await _blobStorageService.DeleteBlobData(command.Content.BlobStorageUrl);
//                }
//                return BadRequest();
//            }
//        }

//        [HttpGet]
//        public async Task<IEnumerable<HelpItemModel>> GetAll()
//        {
//            return await Mediator.Send(new GetAllHelpItemQuery());
//        }

//        [HttpGet("{helpItemId}")]
//        public async Task<HelpItemModel> GetByHelpItemId(string helpItemId)
//        {
//            return await Mediator.Send(new GetHelpItemByIdQuery { HelpItemId = new Guid(helpItemId) });
//        }

//        [HttpGet("{helpItemId}/content")]
//        public async Task<HelpItemContentModel> GetHelpItemContentById(string helpItemId)
//        {
//            // get Help Item by id from request
//            var helpItemModel = await Mediator.Send(new GetHelpItemByIdQuery { HelpItemId = new Guid(helpItemId) });

//            HelpItemContentModel helpItemContentModel = null;
//            if (helpItemModel != null)
//            {
//                // read from blob storage
//                Uri uri = new Uri(helpItemModel.Content.BlobStorageUrl);
//                string filename = Path.GetFileName(uri.LocalPath);
//                CloudBlob file = _blobStorageService.BlobContainer.GetBlobReference(filename);

//                if (await file.ExistsAsync())
//                {
//                    Stream blobStream = file.OpenReadAsync().Result;
//                    using var reader = new StreamReader(blobStream);
//                    helpItemContentModel = new HelpItemContentModel
//                    {
//                        Content = reader.ReadToEnd()
//                    };
//                }
//            }
//            return helpItemContentModel;
//        }

//        [HttpPut("{helpItemId}")]
//        public async Task<ActionResult> UpdateHelpItem(string helpItemId, UpdateHelpItemCommand command)
//        {
//            if (!helpItemId.Equals(command.Id.ToString()))
//            {
//                return BadRequest();
//            }

//            try
//            {
//                // remove old file from blob storage
//                HelpItemModel helpItemModel = await Mediator.Send(new GetHelpItemByIdQuery
//                {
//                    HelpItemId = command.Id
//                });

//                if (helpItemModel != null && !string.IsNullOrEmpty(helpItemModel.Content.BlobStorageUrl))
//                {
//                    await _blobStorageService.DeleteBlobData(helpItemModel.Content.BlobStorageUrl);
//                }

//                command.Content.BlobStorageUrl = await _blobStorageService
//                    .UploadFileToBlobAsync(command.Content.LocalUrl.Replace("\\\\", "\\"), command.Content.FileStorageName);

//                await Mediator.Send(command);
//            }
//            catch (Exception)
//            {
//                // if exception throws and file was already created in blog storage then this file has to be removed
//                if (!string.IsNullOrEmpty(command.Content.BlobStorageUrl))
//                {
//                    await _blobStorageService.DeleteBlobData(command.Content.BlobStorageUrl);
//                }
//                return BadRequest();
//            }

//            return NoContent();
//        }

//        [HttpDelete("{helpItemId}")]
//        public async Task<ActionResult> Delete(string helpItemId)
//        {
//            // remove old file from blob storage
//            HelpItemModel helpItemModel = await Mediator.Send(new GetHelpItemByIdQuery
//            {
//                HelpItemId = new Guid(helpItemId)
//            });

//            if (helpItemModel != null && !string.IsNullOrEmpty(helpItemModel.Content.BlobStorageUrl))
//            {
//                await _blobStorageService.DeleteBlobData(helpItemModel.Content.BlobStorageUrl);
//            }

//            await Mediator.Send(new DeleteHelpItemCommand { Id = new Guid(helpItemId) });

//            return NoContent();
//        }
//    }
//}
