using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using ColabSpace.WebAPI.Models;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Message
{
    public class PinMessage : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PinMessage(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidMessageId_ShouldRaiseBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var messageId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");
            PinAttachFileCommand command = new PinAttachFileCommand()
            {
                MessageId = Guid.NewGuid(),
                BlobStorageUrl = "",
                IsPinFile = true
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Messages/pin-attached-file/{messageId}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
