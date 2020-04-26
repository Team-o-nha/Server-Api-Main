using ColabSpace.Application.TaskItems.Commands.CreateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Guid userId = Guid.NewGuid();

        public Create(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidCreateTaskItemCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var command = new CreateTaskItemCommand
            {
                Name = "Task ATask ATask A",
                Description = "Task ATask ATask A",
                Assignee = new UserModel
                {
                    UserId = userId,
                    DisplayName = "Test user"
                },
                Status = 1,
                CreatedBy = new UserModel
                {
                    UserId = userId,
                    DisplayName = "Test user"
                },
                AttachFiles = new List<AttachFileModel>() { },
                TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae")
            };
            var content = IntegrationTestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/TaskItems", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenInvalidCreateTaskItemCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var usertest = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var command = new CreateTaskItemCommand
            {
                Name = null,
                Description = "Task ATask ATask A",
                Assignee = usertest,
                CreatedBy = usertest,
                AttachFiles = new List<AttachFileModel>() { },
                Status = 1,
                TeamId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae")
            };
            var content = IntegrationTestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/TaskItems", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
