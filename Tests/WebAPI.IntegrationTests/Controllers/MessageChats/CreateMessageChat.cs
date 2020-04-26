using ColabSpace.Application.MessageChats.Commands.CreateMessageChat;
using ColabSpace.WebAPI.IntegrationTests.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.MessageChats
{
    public class CreateMessageChat : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateMessageChat(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateMessageChatCommand
            {
                Content = "hello there",
                RegUserName = "user name",
                ConversationId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                RegUserId = "020cdee0-8ecd-408a-b662-cd4d9cdf0100"
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/MessageChat", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
