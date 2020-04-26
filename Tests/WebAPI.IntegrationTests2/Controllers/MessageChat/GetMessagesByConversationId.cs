using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.MessageChat
{
    public class GetMessagesByConversationIdTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetMessagesByConversationIdTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUserId_ShouldReturn()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var conversationId = Guid.Parse("020cdee0-8ecd-408a-b662-cd4d9cdf0102");

            var response = await client.GetAsync($"/api/MessageChat/GetByConversation/{conversationId}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            // release DB
            _factory.DisposeDbForTests(context);
        }

    }
}
