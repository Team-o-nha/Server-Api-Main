using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Queries.GetConversation;
using ColabSpace.Application.Conversations.Queries.GetConversationsByName;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Domain.Entities;
using ColabSpace.Infrastructure.Persistence;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Conversations.Queries.GetConversationByName
{
    [Collection("QueryCollection")]
    public class GetConversationsByNameQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly string validKeyword = "Group";

        private readonly GetConversationQueryHandler _sut;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public GetConversationsByNameQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
        }

        [Fact]
        public async Task GiveValidNameAndLoginUserJoinedConversation_ShouldRaiseConversationWithNameContainKeyword()
        {
            var sut = new GetConversationsByNameQueryHandler(_context, _mapper);

            var command = new GetConversationsByNameQuery()
            {
                ConversationName = validKeyword,
                LoginUserId = Guid.Parse(_currentUserServiceMock.Object.UserId)
            };

            var result =await sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeEmpty();
            foreach (var con in result)
            {
                con.Name.ShouldContain(validKeyword);
                con.Members.ShouldContain(x => x.UserId == Guid.Parse(_currentUserServiceMock.Object.UserId));
            }
        }

        [Fact]
        public async Task GiveValidNameAndLoginUserNOTJoinedAnyConversation_ShouldRaiseEmptyResult()
        {
            var sut = new GetConversationsByNameQueryHandler(_context, _mapper);

            var command = new GetConversationsByNameQuery()
            {
                ConversationName = validKeyword,
                LoginUserId = Guid.NewGuid()
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeEmpty();
        }
    }
}
