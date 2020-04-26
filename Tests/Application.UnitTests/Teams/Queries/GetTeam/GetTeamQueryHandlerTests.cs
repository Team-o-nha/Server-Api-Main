using AutoMapper;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.Teams.Queries.GetTeamsList;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Queries.GetTeam
{
    [Collection("QueryCollection")]
    public class GetTeamQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid userId = ColabSpaceDbContextFactory.userId1;

        public GetTeamQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetTeamsTest()
        {
            var sut = new GetTeamsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTeamsQuery(), CancellationToken.None);

            result.Count().ShouldBe(3);

        }

        [Fact]
        public async Task GetTeamsByUserIdTest()
        {
            /*var sut = new GetTeamsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTeamsQuery{ UserId = userId.ToString() }, CancellationToken.None);

            result.Count().ShouldBe(1);*/

        }

        [Fact]
        public async Task GetTeamsByTeamNameKeywordTest()
        {
            /*var sut = new GetTeamsQueryHandler(_context, _mapper);
            string keyword = "Team";

            var result = await sut.Handle(new GetTeamsQuery { TeamName = keyword }, CancellationToken.None);

            result.Count().ShouldBe(3);*/

        }
    }
}
