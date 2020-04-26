using AutoMapper;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.Planners.Queries.GetPlannersList;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Planners.Queries.GetPlannersListQueriesHandlerTests2
{
    [Collection("QueryCollection")]
    public class GetPlannersListQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid twoPlanner_TeamId = ColabSpaceDbContextFactory.teamId1;

        public GetPlannersListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPlanners_GiveKeyword_ListIsEmpty_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = twoPlanner_TeamId,
                Keyword = "w"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task GetPlanners_GiveKeyword_ReturnList_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = twoPlanner_TeamId,
                Keyword = "Planner"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
            foreach (PlannerModel planner in result)
            {
                planner.Title.ShouldContain("Planner");
            }
        }

    }
}

