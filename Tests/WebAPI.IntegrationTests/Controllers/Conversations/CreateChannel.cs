using ColabSpace.Application.Conversations.Commands.CreateChannelConversation;
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
    public class CreateChannel : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateChannel(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidTeamId_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateChannelConversationCommand
            {
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
                },
                IsPublic = true,
                Name = "test",
                TeamId = Guid.NewGuid().ToString()
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/conversation/CreateChannel", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenDupplicateName_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateChannelConversationCommand
            {
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
                },
                IsPublic = true,
                Name = "duplicate",
                TeamId = Guid.NewGuid().ToString()
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/conversation/CreateChannel", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidTeamId_EnsureSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreateChannelConversationCommand
            {
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
                },
                IsPublic = true,
                Name = "test",
                TeamId = "197d0438-e04b-453d-b5de-eca05960c6ae"
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/conversation/CreateChannel", content);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
