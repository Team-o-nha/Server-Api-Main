using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Commands.CreateChannelConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Conversations.Commands.CreateChannel
{
    [Collection("QueryCollection2")]
    public class CreateChannelCommandHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validTeamId2 = ColabSpaceDbContextFactory.teamId2;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid userId2 = ColabSpaceDbContextFactory.userId2;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public CreateChannelCommandHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
        }

        [Fact]
        public async Task GiveInvalidTeamId_ShouldRaiseNotFound()
        {
            var sut = new CreateChannelConversationCommandHandler(_context, _mapper);
            List<UserModel> members = new List<UserModel>();
            members.AddRange(new[] {
                new UserModel {
                    UserId = userId1,
                    DisplayName = "User 1"
                },
                new UserModel {
                    UserId = userId2,
                    DisplayName = "User 2"
                }
            });
            await Should.ThrowAsync<NotFoundException>(() =>
               sut.Handle(new CreateChannelConversationCommand
               {
                   TeamId = Guid.NewGuid().ToString(),
                   ChannelDescription = "Channel test 1",
                   IsPublic = false,
                   Members = members,
                   Name = "NAME ABC",

               }, CancellationToken.None));
        }

        [Fact]
        public async Task GiveValidRequestWithIsNOTPublic_ShouldCreateConversationChannel()
        {
            var sut = new CreateChannelConversationCommandHandler(_context, _mapper);
            List<UserModel> members1 = new List<UserModel>();
            members1.AddRange(new[] {
                new UserModel {
                    UserId = userId1,
                    DisplayName = "User 1"
                },
                new UserModel {
                    UserId = userId2,
                    DisplayName = "User 2"
                }
            });
            // test private channel ->  IsPublic = false
            var command = new CreateChannelConversationCommand()
            {
                TeamId = validTeamId2.ToString(),
                ChannelDescription = "Channel test 1",
                IsPublic = false,
                Members = members1,
                Name = "TestisPublicFalse",
            };

            await sut.Handle(command, CancellationToken.None);

            var entity = await _context.Conversations.Where(e => e.Name == "TestisPublicFalse").FirstOrDefaultAsync();

            entity.ShouldNotBeNull();
            entity.TeamId.ShouldBe(validTeamId2.ToString());
            entity.isPublic.ShouldBeFalse();
            entity.Type.ShouldBe("channel");

            // Duplicate Member list??? dont know why

            var temp = entity.Members.GroupBy(e => e.UserOid).Select(group => group.First());
            temp.Count().ShouldBe(2);
            temp.Where(e => e.UserOid == userId1.ToString()).Count().ShouldBe(1);
            temp.Where(e => e.UserOid == userId2.ToString()).Count().ShouldBe(1);
        }
    }

    [Collection("QueryCollection3")]
    public class CreateChannelCommandHandlerTests2
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validTeamId = ColabSpaceDbContextFactory.teamId1;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid userId2 = ColabSpaceDbContextFactory.userId2;
        private readonly Guid userId3 = ColabSpaceDbContextFactory.userId3;
        private readonly Guid userId4 = ColabSpaceDbContextFactory.userId4;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public CreateChannelCommandHandlerTests2(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
        }

        [Fact]
        public async Task GiveValidRequestWithIsPublic_ShouldCreateConversationChannel()
        {
            var sut = new CreateChannelConversationCommandHandler(_context, _mapper);

            // test general channel ->  IsPublic = true
            var command = new CreateChannelConversationCommand()
            {
                TeamId = validTeamId.ToString(),
                ChannelDescription = "Channel test 1",
                IsPublic = true,
                Name = "NAME ABC2",
            };

            await sut.Handle(command, CancellationToken.None);

            var entity = await _context.Conversations.Where(e => e.Name == "NAME ABC2").FirstOrDefaultAsync();

            entity.ShouldNotBeNull();
            entity.TeamId.ShouldBe(validTeamId.ToString());
            entity.isPublic.ShouldBeTrue();
            entity.Type.ShouldBe("channel");

            // Duplicate Member list??? dont know why

            var temp = entity.Members.GroupBy(e => e.UserOid).Select(group => group.First());
            temp.Count().ShouldBe(4);
            temp.Where(e => e.UserOid == userId1.ToString()).Count().ShouldBe(1);
            temp.Where(e => e.UserOid == userId2.ToString()).Count().ShouldBe(1);
            temp.Where(e => e.UserOid == userId3.ToString()).Count().ShouldBe(1);
            temp.Where(e => e.UserOid == userId4.ToString()).Count().ShouldBe(1);
        }
    }
}
