using ColabSpace.Application.Notifications.Command.CreateNotification;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Notifications
{
    public class CreateNotification : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateNotification(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ShouldCreateNotification()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validUserId = new Guid("66EDB7C7-11BF-40A5-94AB-75A3364FEF60");
            var command = new CreateNotificationCommand()
            {
                MessageContent = "message 2 @USER2",
                ConversationId = Guid.NewGuid().ToString(),
                MessageId = Guid.NewGuid().ToString(),
                RegUserName = "USER 1",
                isRead = false,
                ToUser = new List<UserModel>() { new UserModel { UserId = new Guid("66edb7c7-11bf-40a5-94ab-75a3364fef60"), DisplayName = "USER2" } },
                Type = "Mention"
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/Notifications/", content);

            response.EnsureSuccessStatusCode();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidType_ReturnException()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validUserId = new Guid("66EDB7C7-11BF-40A5-94AB-75A3364FEF60");
            var command = new CreateNotificationCommand()
            {
                MessageContent = "message 2 @USER2",
                ConversationId = "b73477a4-f61d-46fa-873c-7d71c01dfbdf",
                MessageId = "b73477a4-f61d-46fa-873c-7d71c01dfbdf",
                RegUserName = "USER 1",
                isRead = false,
                ToUser = new List<UserModel>() { new UserModel { UserId = new Guid("66edb7c7-11bf-40a5-94ab-75a3364fef60"), DisplayName = "USER2" } },
                Type = "123ABC"
            };
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/Notifications/", content);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
