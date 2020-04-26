using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Conversations
{
    public class GetByMembers : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetByMembers(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidUserId_ReturnConversationMessageModel_HaveMessageList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var command = new CreateConversationCommand()
            {
                Type = "pair",
                Members = new List<UserModel>()
                    {
                        new UserModel()
                        {
                            UserId = new Guid("9c7ff9c5-90bd-4207-9dff-01da2ceece21"),
                            DisplayName = "TestUser4"
                        },
                        new UserModel()
                        {
                            UserId = new Guid("020cdee0-8ecd-408a-b662-cd4d9cdf0100"),
                            DisplayName = "TestUser5"
                        }
                    },
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/Conversation/GetConversationByMembers/", content);

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<ConversationMessageModel>(response);

            vm.ShouldBeOfType<ConversationMessageModel>();
            vm.Conversation.Id.ShouldBe(new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbd2"));
            vm.LstMessageChat.Count.ShouldBe(1);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidUserId_ReturnHttpStatusCodeOK_andNewConversationWithEmptyListMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var command = new CreateConversationCommand()
            {
                Type = "pair",
                Members = new List<UserModel>()
                    {
                        new UserModel()
                        {
                            UserId = new Guid("020cdee0-8ecd-408a-b662-cd4d9cdf0100"),
                            DisplayName = "TestUser1"
                        },
                        new UserModel()
                        {
                            UserId = Guid.NewGuid(),
                            DisplayName = "TestUser2"
                        }
                    },
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/Conversation/GetConversationByMembers/", content);

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<ConversationMessageModel>(response);

            vm.ShouldBeOfType<ConversationMessageModel>();
            vm.Conversation.ShouldNotBeNull();
            vm.Conversation.Id.ShouldBe(Guid.Empty);
            vm.LstMessageChat.Count.ShouldBe(0);
            // release DB
            _factory.DisposeDbForTests(context);
        }

    }
}
