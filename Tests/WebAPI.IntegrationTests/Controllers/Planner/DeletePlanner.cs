using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Planner
{
    public class DeletePlanner : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public DeletePlanner(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ShouldDeletePlanner()
        {

            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = new Guid("CBB85A08-ED54-4924-9135-E1F723A2BA6B");

            var response = await client.DeleteAsync($"/api/Planner/{validId}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsNotFound()
        {
            var invalidId = Guid.NewGuid();

            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.DeleteAsync($"/api/Planner/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
