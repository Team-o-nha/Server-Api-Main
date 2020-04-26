using System.Threading.Tasks;
using Xunit;
using Shouldly;
using ColabSpace.WebAPI.IntegrationTests.Common;
using ColabSpace.Application.Teams.Models;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class GetAllByUser : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAllByUser(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsTeamsListVm()
        {
            /*var client = await _factory.GetAuthenticatedClientAsync();

            string loginUserId = "020cdee0-8ecd-408a-b662-cd4d9cdf0100";

            var response = await client.GetAsync($"/api/Team/GetAll/{loginUserId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TeamModel>>(response);

            vm.ShouldBeOfType<List<TeamModel>>();
            vm.Count.ShouldBe(3);*/
        }
    }
}
