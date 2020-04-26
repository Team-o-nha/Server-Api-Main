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
    public class Delete : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Delete(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidConversationId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidConversationId = Guid.NewGuid();

            var response = await client.DeleteAsync($"/api/Conversation/{invalidConversationId.ToString()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenConversationIdIsNotChannel_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validConversationId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            var response = await client.DeleteAsync($"/api/Conversation/{validConversationId.ToString()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenValidconversationId_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validConversationId = Guid.Parse("CF7F4DE0-58B8-4CE1-8758-055706A41BE7");

            var response = await client.DeleteAsync($"/api/Conversation/{validConversationId.ToString()}");

            response.EnsureSuccessStatusCode();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
