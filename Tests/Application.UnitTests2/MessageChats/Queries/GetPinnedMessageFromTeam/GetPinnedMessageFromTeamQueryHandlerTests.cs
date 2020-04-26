using AutoMapper;
using ColabSpace.Application.MessageChats.Queries.GetPinnedMessageFromTeam;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using System;
using System.Linq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.MessageChats.Queries.GetPinnedMessageFromTeam
{
    [Collection("QueryCollection")]
    public class GetPinnedMessageFromTeamQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        private readonly Guid onepinnedTeamId = ColabSpaceDbContextFactory.teamId1;
        private readonly Guid nopinnedTeamId = ColabSpaceDbContextFactory.teamId2;

        public GetPinnedMessageFromTeamQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GiveTeamId_NoPageIndex_ReturnEmptyList()
        {
            var sut = new GetPinnedMessageFromTeamQueryHandler(_context, _mapper);

            var command = new GetPinnedMessageFromTeamQuery()
            {
                TeamId = nopinnedTeamId
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task GiveTeamId_NoPageIndex_ReturnOnePinnedMessage()
        {
            var sut = new GetPinnedMessageFromTeamQueryHandler(_context, _mapper);

            var command = new GetPinnedMessageFromTeamQuery()
            {
                TeamId = onepinnedTeamId
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.Count().ShouldBe(1);
        }
    }
}
