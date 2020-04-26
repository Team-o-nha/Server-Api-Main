using ColabSpace.Application.Conversations.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Conversations
{
    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAll(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        //[Fact]
        //public async Task GivenValidUserId_ReturnConversationsListVm()
        //{
        //    var client = await _factory.GetAuthenticatedClientAsync();

        //    var validUserId = Guid.Parse("9a1709db-fddf-4f91-8046-e3bc92316bab");

        //    var response = await client.GetAsync($"/api/Conversation/GetAll/{validUserId}");

        //    response.EnsureSuccessStatusCode();

        //    var vm = await IntegrationTestHelper.GetResponseContent<List<ConversationModel>>(response);

        //    vm.ShouldBeOfType<List<ConversationModel>>();
        //    vm.Count.ShouldBe(1);
        //}

        //[Fact]
        //public async Task GivenInvalidUserId_ReturnHttpStatusCodeOK_andEmptyList()
        //{
        //    var client = await _factory.GetAuthenticatedClientAsync();

        //    var invalidUserId = Guid.NewGuid().ToString();

        //    var response = await client.GetAsync($"/api/Conversation/GetAll/{invalidUserId}");

        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);

        //    var vm = await IntegrationTestHelper.GetResponseContent<List<ConversationModel>>(response);

        //    vm.ShouldBeOfType<List<ConversationModel>>();

        //    vm.Count.ShouldBe(0);
        //}

    }
}
