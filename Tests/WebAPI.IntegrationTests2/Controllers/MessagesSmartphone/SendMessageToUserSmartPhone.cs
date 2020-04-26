using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using ColabSpace.WebAPI.Models;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Message
{
    public class SendMessageToUserSmartPhone : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SendMessageToUserSmartPhone(CustomWebApplicationFactory<Startup> factory)
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

            var content = IntegrationTestHelper.GetRequestContent(newMessage);

            var response = await client.PostAsync($"/api/MessagesSmartphone/SendMessageToUser/{receiverUserId}", content);

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

            var content = IntegrationTestHelper.GetRequestContent(newMessage);

            var response = await client.PostAsync($"/api/MessagesSmartphone/SendMessageToGroup/{validConversationGroupType}", content);

            response.EnsureSuccessStatusCode();

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
