using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Search
{
    public class SearchUsersAndTeams : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SearchUsersAndTeams(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidTeamId_ReturnsTaskItemsListVm()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            //var keyword = "T";

            //var response = await client.GetAsync($"/api/Search/userAndTeam/{keyword}");

            //response.EnsureSuccessStatusCode();

            //var vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            //vm.ShouldBeOfType<List<SearchModel>>();
            //vm.Count.ShouldBe(7);
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsEmptyList()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            //var keyword = "invalid";

            //var response = await client.GetAsync($"/api/Search/userAndTeam/{keyword}");

            //response.EnsureSuccessStatusCode();

            //var vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            //vm.Count.ShouldBe(0);
        }
    }
}
