using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class Get : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Get(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidId_ReturnsTeamModel()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/Team/GetTeam/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<TeamModel>(response);

            vm.ShouldBeOfType<TeamModel>();
            vm.Id.ShouldBe(validId);
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var invalidId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Team/GetTeam/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
