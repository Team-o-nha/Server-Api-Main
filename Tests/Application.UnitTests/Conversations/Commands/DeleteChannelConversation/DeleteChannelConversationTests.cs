using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Commands.DeleteChannelConversation;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatList;
using ColabSpace.Application.UnitTests.Common;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.DeleteChannelConversation
{
    public class DeleteChannelConversationTests : CommandTestBase
    {
        public static Guid creatorId = ColabSpaceDbContextFactory.userId2;
        //public static Guid assigneeId = ColabSpaceDbContextFactory.userId3;
        public static Guid memberId = ColabSpaceDbContextFactory.userId4;
        
        public static Guid leaderId = ColabSpaceDbContextFactory.userId1;
        public static Guid validTeamId = ColabSpaceDbContextFactory.teamId1;
        public static Guid leaderChannelId = ColabSpaceDbContextFactory.channelId1;
        public static Guid memberChannelId = ColabSpaceDbContextFactory.channelId2;
        public static Guid teamNotExistConversationId = ColabSpaceDbContextFactory.channelId4;

        private DeleteChannelConversationCommandHandler _sut;

        private DeleteChannelConversationCommandHandler SetLoginUser(Guid loginUserId)
        {
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(loginUserId.ToString());

            return new DeleteChannelConversationCommandHandler(_context, currentUserServiceMock.Object);
        }

        /**
         * Given task item id equals null throws ArgumentNullException
         */
        [Fact]
        public async Task Handle_GivenInvalidConversationId_ThrowsNotFoundException()
        {
            // Login user is creator
            _sut = SetLoginUser(leaderId);

            var invalidTaskId = Guid.NewGuid();

            var command = new DeleteChannelConversationCommand { ConversationId = invalidTaskId.ToString() };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given conversation exist, team does not exist
         * throws NotFoundException
         */
        [Fact]
        public async Task Handle_GivenValidConversationId_TeamIdNotExist_ThrowsNotFoundException()
        {
            // Login user is creator
            _sut = SetLoginUser(leaderId);

            var command = new DeleteChannelConversationCommand { ConversationId = teamNotExistConversationId.ToString() };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidConversationId_LoginUserIsNotCreaterAndNotLeader_ThrowsNotOwnedException()
        {
            // Login user is member but is not creator and is not assignee
            _sut = SetLoginUser(memberId);

            var command = new DeleteChannelConversationCommand { ConversationId = leaderChannelId.ToString() };

            await Assert.ThrowsAsync<NotOwnedException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given: conversation exist, team exist
         * Login user is not creator but is leader
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidConversationIdAndValidTeamId_LoginUserIsLeader_DeletesConversation()
        {
            // Login user is leader
            _sut = SetLoginUser(leaderId);

            var command = new DeleteChannelConversationCommand { ConversationId = memberChannelId.ToString() };

            await _sut.Handle(command, CancellationToken.None);

            var channel = await _context.Conversations.FindAsync(memberChannelId);

            // find all message chat
            var messageChats = await new GetMessageChatsQueryHandler(_context, _mapper)
                .Handle(new GetMessageChatsQuery { ConversationId = memberChannelId }, CancellationToken.None);

            Assert.Null(channel);
            messageChats.Count().ShouldBe(0);
        }

        /**
         * Given: conversation exist, team exist
         * Login user is creator
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidConversationIdAndValidTeamId_LoginUserIsCreator_DeletesConversation()
        {
            // Login user is leader
            _sut = SetLoginUser(creatorId);

            var command = new DeleteChannelConversationCommand { ConversationId = memberChannelId.ToString() };

            await _sut.Handle(command, CancellationToken.None);

            var channel = await _context.Conversations.FindAsync(memberChannelId);

            // find all message chat
            var messageChats = await new GetMessageChatsQueryHandler(_context, _mapper)
                .Handle(new GetMessageChatsQuery { ConversationId = memberChannelId }, CancellationToken.None);

            Assert.Null(channel);
            messageChats.Count().ShouldBe(0);
        }
    }
}
