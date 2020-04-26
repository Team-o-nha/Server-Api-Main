using ColabSpace.Application.Planners.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Planner
{
    public class GetPlannerById : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetPlannerById(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidPlannerId_ReturnsPlanner()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("CBB85A08-ED54-4924-9135-E1F723A2BA6B");

            var response = await client.GetAsync($"/api/Planner/GetById/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<PlannerModel>(response);

            vm.ShouldBeOfType<PlannerModel>();
            vm.Id.ShouldBe(validId);
        }

        [Fact]
        public async Task GivenInValidRelatedMessageId_ReturnsNull()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Planner/GetById/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
