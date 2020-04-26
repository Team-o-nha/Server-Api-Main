//using ColabSpace.Application.HelpItems.Commands.CreateHelpItem;
//using ColabSpace.Application.TaskItems.Models;
//using ColabSpace.WebAPI.IntegrationTests.Common;
//using Shouldly;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.WebAPI.IntegrationTests.Controllers.HelpItem
//{
//    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly CustomWebApplicationFactory<Startup> _factory;

//        public Create(CustomWebApplicationFactory<Startup> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task GivenNotFoundContentFile_ReturnsBadRequest()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();

//            var command = new CreateHelpItemCommand
//            {
//                Name = "help item name",
//                Description = string.Empty,
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            var content = IntegrationTestHelper.GetRequestContent(command);

//            var response = await client.PostAsync($"/api/HelpItem", content);

//            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//        }
//    }
//}
