using ColabSpace.Application.TaskItems.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class Get : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Get(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidId_ReturnsTaskItemModel()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/TaskItems/GetTaskItem/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<TaskItemModel>(response);

            vm.ShouldBeOfType<TaskItemModel>();
            vm.Id.ShouldBe(validId);
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var invalidId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/TaskItems/GetTaskItem/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
