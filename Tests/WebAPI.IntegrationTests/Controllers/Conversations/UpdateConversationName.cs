using ColabSpace.Application.Conversations.Commands.UpdateConversationName;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Conversations
{
    public class UpdateConversationName : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateConversationName(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenConversationIdDifferId_ReturnsBadrequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateConversationNameCommand
            {
                Id = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                Name = "Conversation rename",
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Conversation/{Guid.NewGuid()}/name", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidUpdateConversationNameCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateConversationNameCommand
            {
                Id = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"),
                Name = "Conversation rename",
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Conversation/{command.Id}/name", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
