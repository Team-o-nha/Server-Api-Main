using ColabSpace.Application.TaskItems.Commands.UpdateTaskItem;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Update(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUpdateTaskItemCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateTaskItemCommand
            {
                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                Name = "TaskItem 1",
                Description = "Des11111",
                CreatedBy = new UserModel()
                {
                    UserId = Guid.NewGuid(),
                    DisplayName = "TestUser1"
                },
               TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
               Status = 2
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/Update/{command.Id}", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenTaskItemIdDifferId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateTaskItemCommand
            {
                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                Name = "TaskItem 1",
                Description = "Des11111",
                CreatedBy = new UserModel()
                {
                    UserId = Guid.NewGuid(),
                    DisplayName = "TestUser1"
                },
                TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                Status = 2
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/Update/{Guid.NewGuid()}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
