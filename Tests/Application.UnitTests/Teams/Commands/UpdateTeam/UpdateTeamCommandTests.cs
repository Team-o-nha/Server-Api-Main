using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Commands.UpdateTeam;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandTests : CommandTestBase
    {
        private readonly UpdateTeamCommandHandler _sut;

        private readonly Guid validId = ColabSpaceDbContextFactory.teamId1;

        private static Guid leaderId = ColabSpaceDbContextFactory.userId1;
        private static Guid memberId = ColabSpaceDbContextFactory.userId2;
        private static Guid userId3 = ColabSpaceDbContextFactory.userId3;
        private static Guid userId4 = ColabSpaceDbContextFactory.userId4;

        public UpdateTeamCommandTests() : base()
        {
            // Login user is leader
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(leaderId.ToString());
            _sut = new UpdateTeamCommandHandler(_context, currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedTeam()
        {
            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = "Change team name 1.",
                Description = "New description.",
                Users = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            await _sut.Handle(command, CancellationToken.None);

            var entity = _context.Teams.Find(new Guid(command.Id));

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
            entity.Users.ShouldNotBeNull();
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdateTeamCommand
            {
                Id = "99",
                Name = "This item doesn't exist.",
                Description = "This item doesn't exist."
            };


            Should.ThrowAsync<NotFoundException>(() =>
                _sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidId_LoginUserIsNotLeader_ThrowsException()
        {
            // Login user is member
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(memberId.ToString());
            var sut = new UpdateTeamCommandHandler(_context, currentUserServiceMock.Object);

            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = "Change team name 1.",
                Description = "New description.",
                Users = new List<UserModel>
                {
                    new UserModel()
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "TestUser1"
                    }
                }
            };

            await Should.ThrowAsync<NotOwnedException>(() =>
                 sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenNewMember_ShouldUpdatePublicChannels()
        {
            // Arrange
            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = "Change team name 1.",
                Description = "New description.",
                Users = new List<UserModel>
                {
                    // old members
                    new UserModel
                    {
                        UserId = leaderId,
                        DisplayName = "User1",
                        TeamRole = "Leader"
                    },
                    new UserModel
                    {
                        UserId = memberId,
                        DisplayName = "User2",
                        TeamRole = "Member"
                    },
                    new UserModel
                    {
                        UserId = userId3,
                        DisplayName = "User3",
                        TeamRole = "Member"
                    },
                    new UserModel
                    {
                        UserId = userId4,
                        DisplayName = "User4",
                        TeamRole = "Member"
                    },
                    // new member
                    new UserModel
                    {
                        UserId = new Guid(),
                        DisplayName = "new-member",
                        TeamRole = "Member"
                    },
                }
            };

            await _sut.Handle(command, CancellationToken.None);

            var afterUpdateMembers = (await _context.Teams.FindAsync(validId)).Users;

            var lstPublicChannels = _context.Conversations
                .Where(con => con.isPublic && con.TeamId == validId.ToString());

            foreach(var publicChannel in lstPublicChannels)
            {
                publicChannel.Members.ShouldBe(afterUpdateMembers);
            }
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedTeam_User2IsDeleteOfTeam()
        {
            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = "Change team name 1.",
                Description = "New description.",
                Users = new List<UserModel>
                {
                        new UserModel
                        {
                            UserId = leaderId,
                            DisplayName = "User1",
                            TeamRole = "Leader"
                        },
                        new UserModel
                        {
                            UserId = userId3,
                            DisplayName = "User3",
                            TeamRole = "Member"
                        }
                }
            };

            var channelsBeforeUpdateTeam = _context.Conversations.Where(channel => channel.TeamId == validId.ToString()).ToList();

            foreach (var channel in channelsBeforeUpdateTeam)
            {
                channel.Members.Any(member => member.UserOid.Equals(memberId.ToString())).ShouldBeTrue();
            }

            await _sut.Handle(command, CancellationToken.None);

            var entity = _context.Teams.Find(new Guid(command.Id));

            var channelsAfterUpdateTeam = _context.Conversations.Where(channel => channel.TeamId == validId.ToString()).ToList();

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
            entity.Users.ShouldNotBeNull();

            foreach (var channel in channelsAfterUpdateTeam)
            {
                channel.Members.Any(member => member.UserOid.Equals(memberId.ToString())).ShouldBeFalse();
            }
        }
    }
}