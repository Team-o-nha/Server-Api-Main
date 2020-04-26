using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.MessageChat
{
    public class GetLastContentOfConversationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetLastContentOfConversationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUserId_ShouldReturnConversationLastContent()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var userId = Guid.Parse("020cdee0-8ecd-408a-b662-cd4d9cdf0100");

            var response = await client.GetAsync($"/api/MessageChat/GetLastOf/{userId}");
            var listVm = await IntegrationTestHelper.GetResponseContent<List<ConversationLastContentModel>>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            foreach (ConversationLastContentModel vm in listVm)
            {
                vm.Conversation.Members.ShouldContain(member => member.UserId == userId);
            }
            // release DB
            _factory.DisposeDbForTests(context);

        }

        [Fact]
        public async Task GivenValidUserIdAndTeamId_ShouldReturnChannelLastContentModel()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var userId = Guid.Parse("020cdee0-8ecd-408a-b662-cd4d9cdf0101");
            var teamId = Guid.Parse("ffa8a3e7-50ab-4b10-8e49-96c9f837169d");

            var response = await client.GetAsync($"/api/MessageChat/GetLastOf/{userId}/?teamId={teamId}");

            var listVm = await IntegrationTestHelper.GetResponseContent<List<ConversationLastContentModel>>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            foreach (ConversationLastContentModel vm in listVm)
            {
            vm.Conversation.Members.ShouldContain(member => member.UserId == userId);
            vm.Conversation.TeamId.ShouldBe(teamId.ToString());
            }
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
