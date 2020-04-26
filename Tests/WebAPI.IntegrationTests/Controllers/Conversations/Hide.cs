using ColabSpace.Application.Conversations.Commands.HideConversation;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Conversations
{
    public class Hide : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Hide(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidConversationId_ReturnsBadrequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidConversationId = Guid.NewGuid();

            var content = IntegrationTestHelper.GetRequestContent("");

            var response = await client.PostAsync($"/api/Conversation/Hide/{invalidConversationId.ToString()}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenValidconversationId_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validConversationId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            var content = IntegrationTestHelper.GetRequestContent("");

            var response = await client.PostAsync($"/api/Conversation/Hide/{validConversationId.ToString()}", content);

            response.EnsureSuccessStatusCode();

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}
