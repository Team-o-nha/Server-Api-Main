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
    public class UpdateMessage : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateMessage(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequestWithIsPin_ShouldUpdateMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var messageId = Guid.Parse("b73477a4-f61d-46fa-873c-7d71c01dfbdf");

            UpdateMessageChatCommand command = new UpdateMessageChatCommand()
            {
                MessageId = Guid.Parse("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                IsPin = true,
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Messages/{messageId}", content);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidRequestWithReactionType_ShouldUpdateMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var messageId = Guid.Parse("b73477a4-f61d-46fa-873c-7d71c01dfbdf");

            UpdateMessageChatCommand command = new UpdateMessageChatCommand()
            {
                MessageId = Guid.Parse("b73477a4-f61d-46fa-873c-7d71c01dfbdf"),
                ReactionType = "+1",
            };
            // reactionType ['+1', 'grinning', 'white_frowning_face', 'angry', 'astonished'];
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Messages/{messageId}", content);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidRequest_ShouldRaiseBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var messageId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            UpdateMessageChatCommand command = new UpdateMessageChatCommand()
            {
                MessageId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBD1"),
                IsPin = true,
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Messages/{messageId}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // release DB
            _factory.DisposeDbForTests(context);
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
