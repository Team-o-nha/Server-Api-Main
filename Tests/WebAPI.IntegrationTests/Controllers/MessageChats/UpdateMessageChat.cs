using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.WebAPI.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.MessageChats
{
    public class UpdateMessageChat : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateMessageChat(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateMessageChatCommand
            {
                MessageId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                ReactionType = "Like"
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/MessageChat/{command.MessageId.ToString()}", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
