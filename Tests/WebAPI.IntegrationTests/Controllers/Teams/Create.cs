using ColabSpace.Application.Teams.Commands.CreateTeam;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Create(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidCreateTeamCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateTeamCommand
            {
                Name = "Team A.",
                Description = "Des A",
                Users = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/team", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenInvalidCreateTeamCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateTeamCommand
            {
                Name = "This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length.",
                Description = "Des A",
                Users = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/team", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
