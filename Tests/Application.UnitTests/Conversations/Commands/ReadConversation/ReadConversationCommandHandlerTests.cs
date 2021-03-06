﻿using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Commands.ReadConversation;
using ColabSpace.Application.UnitTests.Common;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.ReadConversation
{

    public class ReadConversationCommandHandlerTests : CommandTestBase
    {
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid conversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid invalidConversationId = Guid.NewGuid();
        DateTime oldLastSeenTime = new DateTime();
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        [Fact]
        public async Task GiveValidConversationId_ShouldReadConversationToLoginUser()
        {
            // mock login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var oldEntity = _context.Conversations.Find(conversationId);
            foreach (var member in oldEntity.Members)
            {
                if (member.UserOid == userId1.ToString())
                {
                    oldLastSeenTime = member.LastSeenTime;
                }
            }

            var sut = new ReadConversationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new ReadConversationCommand
            {
                ConversationId = conversationId.ToString()
            };

            await sut.Handle(command, CancellationToken.None);

            var entity = _context.Conversations.Find(conversationId);

            foreach (var member in entity.Members)
            {
                if (member.UserOid == userId1.ToString())
                {
                    member.LastSeenTime.ShouldBeGreaterThan(oldLastSeenTime);
                }
            }
        }

        [Fact]
        public void GiveInvalidConversationId_ShouldRaiseNotFound()
        {
            // mock login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());

            var sut = new ReadConversationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new ReadConversationCommand
            {
                ConversationId = invalidConversationId.ToString()
            };

            _ = Should.ThrowAsync<NotFoundException>(() =>
                   sut.Handle(command, CancellationToken.None));
        }
    }
}

