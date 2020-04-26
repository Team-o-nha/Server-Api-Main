using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Commands.DeleteTeam;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Commands.DeleteTeam
{
    public class DeleteTeamCommandTests : CommandTestBase
    {
        private readonly DeleteTeamCommandHandler _sut;

        private readonly Guid validId = ColabSpaceDbContextFactory.teamId1;

        private static Guid leaderId = ColabSpaceDbContextFactory.userId1;
        private static Guid memberId = ColabSpaceDbContextFactory.userId2;

        public DeleteTeamCommandTests()
            : base()
        {
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(leaderId.ToString());
            _sut = new DeleteTeamCommandHandler(_context, currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ThrowsNotFoundException()
        {
            var invalidId = Guid.NewGuid();

            var command = new DeleteTeamCommand { Id = invalidId.ToString() };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidIdAndZeroOrders_DeletesTeam()
        {
            var command = new DeleteTeamCommand { Id = validId.ToString() };

            var channelsBeforeDelete = _context.Conversations.Where(t => t.TeamId == validId.ToString()).ToList();

            await _sut.Handle(command, CancellationToken.None);

            var entity =  _context.Teams.Find(validId);

            var taskItems = _context.TaskItems.Where(t => t.TeamId == validId).ToList();

            var channels = _context.Conversations.Where(t => t.TeamId == validId.ToString()).ToList();

            var messageChats = new List<MessageChat>();
            foreach (var channel in channelsBeforeDelete)
            {
                // find MessageChat async (tim cac MessageChat cung mot luc)
                messageChats.AddRange(await _context.MessageChats.Where(t => t.ConversationId == channel.Id).ToListAsync());
            }

            entity.ShouldBeNull();
            taskItems.Count.ShouldBe(0);
            channelsBeforeDelete.Count.ShouldNotBe(0);
            channels.Count.ShouldBe(0);
            messageChats.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenValidId_UserIsNotLeader_ThrowsNotOwnedException()
        {
            // Login user is member
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(memberId.ToString());
            var sut = new DeleteTeamCommandHandler(_context, currentUserServiceMock.Object);

            var command = new DeleteTeamCommand { Id = validId.ToString() };

            await Assert.ThrowsAsync<NotOwnedException>(() => sut.Handle(command, CancellationToken.None));
        }

    }
}
