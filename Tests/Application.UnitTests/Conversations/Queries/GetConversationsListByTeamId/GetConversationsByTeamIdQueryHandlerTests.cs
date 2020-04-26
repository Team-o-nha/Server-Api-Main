using AutoMapper;
using ColabSpace.Application.Conversations.Queries.GetConversationsListByTeamId;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Queries.GetConversationsListByTeamId
{
    [Collection("QueryCollection")]
    public class GetConversationsByTeamIdQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid emptyConversation_TeamId = Guid.NewGuid();
        private readonly Guid oneConversation_TeamId = ColabSpaceDbContextFactory.teamId3;
        private readonly Guid twoConversation_TeamId = ColabSpaceDbContextFactory.teamId1;

        public GetConversationsByTeamIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_GivenTeamIdHaveNoConversation_ReturnListEmpty()
        {
            var sut = new GetConversationsByTeamIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetConversationsByTeamIdQuery
            {
                TeamId = emptyConversation_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenTeamIdHaveOneConversation_ReturnListConversation()
        {
            var sut = new GetConversationsByTeamIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetConversationsByTeamIdQuery
            {
                TeamId = oneConversation_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Handle_GivenTeamIdHaveTwoConversations_ReturnListConversation()
        {
            var sut = new GetConversationsByTeamIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetConversationsByTeamIdQuery
            {
                TeamId = twoConversation_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }
    }
}
