using System.Threading.Tasks;
using Xunit;
using Shouldly;
using ColabSpace.WebAPI.IntegrationTests.Common;
using ColabSpace.Application.Teams.Models;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAll(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsTeamsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync("/api/Team/GetAll");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TeamModel>>(response);

            vm.ShouldBeOfType<List<TeamModel>>();
            vm.Count.ShouldBe(4);
        }
    }
}
