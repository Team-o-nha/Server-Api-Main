using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.UnitTests.Common;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.UpdateMessageChat
{
    public class UpdateMessageChatCommandHandlerTests : CommandTestBase
    {
        public static Guid reactor = ColabSpaceDbContextFactory.userId1;
        public static Guid messageId = ColabSpaceDbContextFactory.messagechannelId1;
        public static Guid messageHasOneReactionId = ColabSpaceDbContextFactory.messagechannelId3;

        private UpdateMessageChatCommandHandler _sut;

        private UpdateMessageChatCommandHandler SetLoginUser(Guid loginUserId)
        {
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(loginUserId.ToString());

            return new UpdateMessageChatCommandHandler(_context, currentUserServiceMock.Object);
        }

        /**
         * Given Message id is invalid throw Not Found Exception
         */
        [Fact]
        public async Task Handle_GivenInvalidMessageId_ThrowsNotFoundException()
        {
            // Login user is Reactor
            _sut = SetLoginUser(reactor);

            var invalidTaskId = Guid.NewGuid();

            var command = new UpdateMessageChatCommand { MessageId = invalidTaskId };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given valid Message Id and ReactionType
         * Add reaction to message
         */
        [Fact]
        public async Task Handle_GivenValidMessageIdAndReactionTypeAndIsDeleteReactionIsFalse_AddReactionSuccess()
        {
            // Login user is Reactor
            _sut = SetLoginUser(reactor);

            var command = new UpdateMessageChatCommand
            {
                MessageId = messageId,
                ReactionType = "Like",
            };

            await _sut.Handle(command, CancellationToken.None);

            var message = await _context.MessageChats.FindAsync(messageId);

            message.ReactionList.Count.ShouldBe(1);
            message.IsPin.ShouldBeFalse();
        }

        /**
         * Given valid Message Id and ReactionType
         * Login User is Reactor, ReactionType is changed
         * change reaction
         */
        [Fact]
        public async Task Handle_GivenValidMessageIdAndReactionTypeAndIsDeleteReactionIsTrue_NotRemoveReactionSuccess()
        {
            // Login user is not reactor
            _sut = SetLoginUser(reactor);

            var command = new UpdateMessageChatCommand
            {
                MessageId = messageHasOneReactionId,
                ReactionType = "Angry",
            };

            await _sut.Handle(command, CancellationToken.None);

            var message = await _context.MessageChats.FindAsync(messageHasOneReactionId);

            message.ReactionList.Count.ShouldBe(1);
            message.IsPin.ShouldBeFalse();
        }

        /**
         * Given valid Message Id and ReactionType, IsAddReaction = true
         * Login User is Reactor
         * Remove reaction to message
         */
        [Fact]
        public async Task Handle_GivenValidMessageIdAndReactionTypeAndIsDeleteReactionIsTrue_RemoveReactionSuccess()
        {
            // Login user is Reactor
            _sut = SetLoginUser(reactor);

            var command = new UpdateMessageChatCommand
            {
                MessageId = messageHasOneReactionId,
                ReactionType = "Like",
            };

            await _sut.Handle(command, CancellationToken.None);

            var message = await _context.MessageChats.FindAsync(messageHasOneReactionId);

            message.ReactionList.Count.ShouldBe(0);
            message.IsPin.ShouldBeFalse();
        }

        /**
         * Given valid Message Id and IsPin has value
         * Login User is Reactor
         * update IsPin
         */
        [Fact]
        public async Task Handle_GivenValidMessageIdAndIsPinHasValue_UpdateSuccess()
        {
            // Login user is Reactor
            _sut = SetLoginUser(reactor);

            var command = new UpdateMessageChatCommand
            {
                MessageId = messageHasOneReactionId,
                IsPin = true
            };

            await _sut.Handle(command, CancellationToken.None);

            var message = await _context.MessageChats.FindAsync(messageHasOneReactionId);

            message.IsPin.ShouldBeTrue();
        }
    }
}
