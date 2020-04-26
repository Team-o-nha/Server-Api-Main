//using ColabSpace.Application.HelpItems.Commands.UpdateHelpItem;
//using ColabSpace.Application.TaskItems.Models;
//using ColabSpace.WebAPI.IntegrationTests.Common;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.WebAPI.IntegrationTests.Controllers.HelpItem
//{
//    public class UpdateHelpItemControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly CustomWebApplicationFactory<Startup> _factory;

//        public UpdateHelpItemControllerTests(CustomWebApplicationFactory<Startup> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task GivenDifferentHelpItemId_ReturnsBadRequest()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();

//            var command = new UpdateHelpItemCommand
//            {
//                Id = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973"),
//                Name = "new name",
//                Description = "new description",
//                Content = new AttachFileModel
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            var content = IntegrationTestHelper.GetRequestContent(command);

//            var response = await client.PutAsync($"/api/HelpItem/{Guid.NewGuid()}", content);

//            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task GivenNotFoundContentFile_ReturnsBadRequest()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();

//            var command = new UpdateHelpItemCommand
//            {
//                Id = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973"),
//                Name = "new name",
//                Description = "new description",
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            var content = IntegrationTestHelper.GetRequestContent(command);

//            var response = await client.PutAsync($"/api/HelpItem/{command.Id}", content);

//            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//        }
//    }
//}
