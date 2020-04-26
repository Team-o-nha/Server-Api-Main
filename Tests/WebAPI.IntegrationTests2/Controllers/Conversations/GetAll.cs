using ColabSpace.Application.Conversations.Models;
using ColabSpace.Infrastructure.Persistence;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Conversations
{
    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAll(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidUserId_ReturnConversationsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var validUserId = Guid.Parse("9c7ff9c5-90bd-4207-9dff-01da2ceece21");

            var response = await client.GetAsync($"/api/Conversation/GetAll/{validUserId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<ConversationModel>>(response);

            vm.ShouldBeOfType<List<ConversationModel>>();
            vm.Count.ShouldBe(2);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidUserId_ReturnHttpStatusCodeOK_andEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var invalidUserId = Guid.NewGuid().ToString();

            var response = await client.GetAsync($"/api/Conversation/GetAll/{invalidUserId}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<List<ConversationModel>>(response);

            vm.ShouldBeOfType<List<ConversationModel>>();

            vm.Count.ShouldBe(0);
            // release DB
            _factory.DisposeDbForTests(context);
        }

    }
}
