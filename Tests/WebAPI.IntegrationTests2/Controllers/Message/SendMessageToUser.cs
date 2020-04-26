using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using ColabSpace.WebAPI.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Message
{
    public class SendMessageToUser : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SendMessageToUser(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidRequest_SendMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var receiverUserId = Guid.Parse("9c7ff9c5-90bd-4207-9dff-01da2ceece21");
            MessageDto newMessage = new MessageDto()
            {
                AttachFiles = null,
                ConversationId = new Guid("b73477a4-f61d-46fa-873c-7d71c01dfbd2"),
                Date = DateTime.UtcNow,
                IsPin = false,
                Message = "ABC123456",
                Type = "received"
            };

            var connection = new HubConnectionBuilder().WithUrl("http://localhost/chathub"
                , options =>
                {
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                    options.AccessTokenProvider = () => _factory.GetAccessTokenByUserAsync("testuser", "testuser");
                }
                )
                .Build();
            await connection.StartAsync();

            var connection1 = new HubConnectionBuilder().WithUrl("http://localhost/chathub"
                , options =>
                {
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                    options.AccessTokenProvider = () => _factory.GetAccessTokenByUserAsync("testuser3", "testuser3");
                }
                )
                .Build();
            await connection1.StartAsync();

            var content = IntegrationTestHelper.GetRequestContent(newMessage);

            var response = await client.PostAsync($"/api/Messages/SendMessageToUser/{receiverUserId}", content);

            response.EnsureSuccessStatusCode();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidRequest_SendMessageToGroup()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var validConversationGroupType = Guid.Parse("dcdb9146-32e5-4cc3-ad1c-d10e05745f02");
            MessageDto newMessage = new MessageDto()
            {
                AttachFiles = null,
                ConversationId = new Guid("dcdb9146-32e5-4cc3-ad1c-d10e05745f02"),
                Date = DateTime.UtcNow,
                IsPin = false,
                Message = "ABC123456",
                Type = "received"
            };

            var connection = new HubConnectionBuilder().WithUrl("http://localhost/chathub"
                , options =>
                {
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                    options.AccessTokenProvider = () => _factory.GetAccessTokenByUserAsync("testuser", "testuser");
                }
                )
                .Build();
            await connection.StartAsync();

            var connection1 = new HubConnectionBuilder().WithUrl("http://localhost/chathub"
                , options =>
                {
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                    options.AccessTokenProvider = () => _factory.GetAccessTokenByUserAsync("testuser3", "testuser3");
                }
                )
                .Build();
            await connection1.StartAsync();

            var content = IntegrationTestHelper.GetRequestContent(newMessage);

            var response = await client.PostAsync($"/api/Messages/SendMessageToGroup/{validConversationGroupType}", content);

            response.EnsureSuccessStatusCode();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidMessageId_ShouldRaiseBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var messageId = Guid.Parse("B73477A4-F61D-46FA-873C-7D71C01DFBDF");
            PinAttachFileCommand command = new PinAttachFileCommand()
            {
                MessageId = Guid.NewGuid(),
                BlobStorageUrl = "",
                IsPinFile = true
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/Messages/pin-attached-file/{messageId}", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
