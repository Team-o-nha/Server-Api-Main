using AutoMapper;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.Teams.Queries.GetTeam;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Teams.Queries.GetTeamDetail
{
    [Collection("QueryCollection")]
    public class GetTeamDetailQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validId = ColabSpaceDbContextFactory.teamId1;

        public GetTeamDetailQueryHandlerTests(QueryTestFixture fixture)
        {
            this._context = fixture.Context;
            this._mapper = fixture.Mapper;
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
