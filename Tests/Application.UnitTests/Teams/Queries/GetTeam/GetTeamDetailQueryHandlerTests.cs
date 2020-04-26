using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using ColabSpace.Infrastructure.Persistence;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Application.Teams.Queries.GetTeam;
using System;
using ColabSpace.Application.Teams.Models;

namespace ColabSpace.Application.UnitTests.Teams.Queries.GetTeam
{
    [Collection("QueryCollection")]
    public class GetTeamDetailQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validId = ColabSpaceDbContextFactory.teamId1;

        public GetTeamDetailQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetTeamDetail()
        {
            var sut = new GetTeamQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTeamQuery { TeamId = validId }, CancellationToken.None);

            result.ShouldBeOfType<TeamModel>();
            result.Id.ShouldBe(validId);
        }
    }
}
