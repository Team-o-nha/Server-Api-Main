using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Search
{
    public class SearchUserInTeam : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SearchUserInTeam(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var keyword = "T";
            var invalidId = Guid.NewGuid().ToString();

            var response = await client.GetAsync($"/api/Search/userInTeam/{invalidId}/?keyword={keyword}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var keyword = "invalid";
            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/Search/userInTeam/{validId}/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<UserModel>>(response);

            vm.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GivenValidTeamId_ReturnsUserListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var keyword = "Test";

            var response = await client.GetAsync($"/api/Search/userInTeam/{validId}/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<UserModel>>(response);

            vm.ShouldBeOfType<List<UserModel>>();
            vm.Count.ShouldBe(1);
        }
    }
}
