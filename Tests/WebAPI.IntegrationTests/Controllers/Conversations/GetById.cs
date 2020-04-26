using ColabSpace.Application.Common.Models;
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
    public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetById(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidConversationId_ReturnConversationMessageModel()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validConversationId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            var response = await client.GetAsync($"/api/Conversation/{validConversationId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<ConversationMessageModel>(response);

            vm.ShouldBeOfType<ConversationMessageModel>();
            vm.Conversation.Id.ShouldBe(new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF"));
            vm.LstMessageChat.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GivenInvalidUserId_ReturnHttpStatusCodeNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidConversationId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Conversation/{invalidConversationId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

    }
}
