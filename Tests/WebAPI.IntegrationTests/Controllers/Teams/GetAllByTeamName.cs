using System.Threading.Tasks;
using Xunit;
using Shouldly;
using ColabSpace.WebAPI.IntegrationTests.Common;
using ColabSpace.Application.Teams.Models;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class GetAllByTeamName : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAllByTeamName(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsTeamsListVm()
        {
            /*var client = await _factory.GetAuthenticatedClientAsync();
            string keword = "Team 1";

            var response = await client.GetAsync($"/api/Team/GetAllByTeamName/{keword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TeamModel>>(response);

            vm.ShouldBeOfType<List<TeamModel>>();
            vm.Count.ShouldBe(1);*/
        }
    }
}
