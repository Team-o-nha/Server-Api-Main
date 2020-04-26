using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.MessageChats.Commands.CreateMessageChat;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.CreateMessageChat
{
    public class CreateMessageChatCommandHandlerTests : CommandTestBase
    {
        private readonly Guid validConversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid loginUser1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid invalidConversationId = Guid.Empty;

        /**
         * Given invalid conversation id (not existing in db)
         * then throws not found exception
         * 
         */
        [Fact]
        public async Task Handle_GivenConversationIdInvalid_ShouldRaiseNotFoundException()
        {
            ////Arrange
            var sut = new CreateMessageChatCommandHandler(_context);

            var command = new CreateMessageChatCommand
            {
                Content = "hello",
                ConversationId = invalidConversationId,
                RegUserName = "user name",
                RegUserId = loginUser1.ToString()
            };

            //// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        /**
         * Login user is not member of conversation
         * then throws not owned exception
         * 
         */
        [Fact]
        public async Task Handle_LoginUserIsNotMemberOfConversation_ShouldRaiseNotOwnedException()
        {
            ////Arrange
            var sut = new CreateMessageChatCommandHandler(_context);

            var command = new CreateMessageChatCommand
            {
                Content = "hello",
                ConversationId = validConversationId,
                RegUserName = "user name",
                RegUserId = ""
            };

            //// Act
            await Assert.ThrowsAsync<NotOwnedException>(() => sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given valid request
         * then should register new MessageChat
         * 
         */
        [Fact]
        public async Task Handle_GivenValidRequest_ShouldRegisSuccess()
        {
            ////Arrange
            var sut = new CreateMessageChatCommandHandler(_context);

            var command = new CreateMessageChatCommand
            {
                Content = "hello",
                ConversationId = validConversationId,
                RegUserName = "user name",
                RegUserId = loginUser1.ToString()
            };

            //// Act
            var messageChatId = await sut.Handle(command, CancellationToken.None);

            var messageChat = await _context.MessageChats.FindAsync(messageChatId);

            messageChat.ConversationId.ShouldBe(validConversationId);
            messageChat.Content.ShouldBe("hello");
        }
    }
}
