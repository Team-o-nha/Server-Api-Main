using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Conversations.Queries.GetConversationsList;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Conversations.Queries.GetConversations
{
    [Collection("QueryCollection")]
    public class GetConversationsListQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid invalidTeamId = Guid.NewGuid();
        private readonly Guid validTeamId = ColabSpaceDbContextFactory.teamId1;

        private readonly GetConversationsQueryHandler _sut;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public GetConversationsListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            // handler
            _sut = new GetConversationsQueryHandler(_context, _mapper);
        }

        [Fact]
        public async Task GiveinValidRequest__ShouldReturnEmptyList()
        {
            var command = new GetConversationsQuery { TeamId = invalidTeamId, UserId = Guid.Parse(_currentUserServiceMock.Object.UserId) };
            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GiveValidRequestWithTeamId__ShouldRaiseConversationsTypeChannel()
        {
            var command = new GetConversationsQuery { TeamId = validTeamId, UserId = Guid.Parse(_currentUserServiceMock.Object.UserId) };
            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<ConversationModel>>();
            foreach (ConversationModel con in result)
            {
                con.TeamId.ShouldBe(validTeamId.ToString());
                con.Type.ShouldBe("channel");
            }
        }

        [Fact]
        public async Task GiveValidRequestWithoutTeamId__ShouldRaiseConversationsTypeNotChannel()
        {
            var command = new GetConversationsQuery { UserId = Guid.Parse(_currentUserServiceMock.Object.UserId) };
            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<ConversationModel>>();
            foreach (ConversationModel con in result)
            {
                con.Type.ShouldNotBe("channel");
            }
        }

        [Fact]
        public async Task GiveValidRequestWithoutTeamIdWithName__ShouldRaiseConversationsTypeNotChannelContainName()
        {
            var command = new GetConversationsQuery { UserId = Guid.Parse(_currentUserServiceMock.Object.UserId), ConversationName = "Grou" };
            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<ConversationModel>>();
            foreach (ConversationModel con in result)
            {
                con.Type.ShouldNotBe("channel");
                con.Name.ShouldContain("Grou");
            }
        }
    }
}
