using ColabSpace.Application.Teams.Commands.UpdateTeam;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Teams
{
    public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Update(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUpdateTeamCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateTeamCommand
            {
                Id = "197d0438-e04b-453d-b5de-eca05960c6ae",
                Name = "Team 1",
                Description = "Des11111",
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

            var response = await client.PutAsync($"/api/Team/{command.Id}", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
