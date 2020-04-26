using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Conversations
{
    public class CreateConversation : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateConversation(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidType_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateConversationCommand
            {
                Type = "invalid",
                Members = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    },
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/conversation", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidType_ReturnsOk()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateConversationCommand
            {
                Type = "pair",
                Members = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    },
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/conversation", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
