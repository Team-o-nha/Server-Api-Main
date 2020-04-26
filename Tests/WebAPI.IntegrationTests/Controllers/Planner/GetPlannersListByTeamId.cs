using ColabSpace.Application.Planners.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Planner
{
    public class GetPlannersListByTeamId : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetPlannersListByTeamId(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidTeamId_ReturnsPlannersList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/Planner/GetAll/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<IEnumerable<PlannerModel>>(response);

            vm.Count().ShouldBe(2);
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Planner/GetAll/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<IEnumerable<PlannerModel>>(response);

            vm.Count().ShouldBe(0);
        }
    }
}
