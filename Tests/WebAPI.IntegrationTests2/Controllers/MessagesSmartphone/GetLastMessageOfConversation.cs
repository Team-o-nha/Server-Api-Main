using ColabSpace.Application.MessageChats.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.MessagesSmartphone
{
    public class GetLastMessageOfConversation : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetLastMessageOfConversation(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidConversationId_ReturnsMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validConversationId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF");
            var validMessageId = new Guid("96149786-A9F7-4B6A-A932-5DDB15A84D1A");

            var response = await client.GetAsync($"/api/MessagesSmartphone/RecentMessage/{validConversationId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<MessageChatModel>(response);

            vm.ShouldBeOfType<MessageChatModel>();
            vm.Id.ShouldBe(validMessageId);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
