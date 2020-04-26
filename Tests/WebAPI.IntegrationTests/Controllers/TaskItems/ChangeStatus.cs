using ColabSpace.Application.TaskItems.Commands.ChangeStatusTaskItem;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class ChangeStatus : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ChangeStatus(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUpdateTaskItemCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new ChangeStatusTaskItemCommand
            {
                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                Status = 2,
                Assignee = new UserModel
                {
                    UserId = Guid.NewGuid(),
                    DisplayName = "AssigneeTest"
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/ChangeStatus/{command.Id}", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenTaskItemIdDifferId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new ChangeStatusTaskItemCommand
            {
                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"),
                Status = 2,
                Assignee = new UserModel
                {
                    UserId = Guid.NewGuid(),
                    DisplayName = "AssigneeTest"
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/ChangeStatus/{Guid.NewGuid()}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
