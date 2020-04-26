using AutoMapper;
using ColabSpace.Application.Planners.Queries.GetPlannersList;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Queries.GetPlannersList
{
    [Collection("QueryCollection")]
    public class GetPlannersListQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid emptyPlanner_TeamId = ColabSpaceDbContextFactory.teamId3;
        private readonly Guid onePlanner_TeamId = ColabSpaceDbContextFactory.teamId2;
        private readonly Guid twoPlanner_TeamId = ColabSpaceDbContextFactory.teamId1;

        public GetPlannersListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPlanners_ListIsEmpty_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = emptyPlanner_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);

        }

        [Fact]
        public async Task GetPlanners_ListCountIsOne_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = onePlanner_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);

        }

        [Fact]
        public async Task GetPlanners_ListCountIsTwo_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = twoPlanner_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);

        }

        [Fact]
        public async Task GetPlanners_HavePageIndex_ListIsEmpty_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = emptyPlanner_TeamId,
                PageIndex = 1
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);

        }

        [Fact]
        public async Task GetPlanners_HavePageIndex_ListCountIsOne_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = onePlanner_TeamId,
                PageIndex = 1
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);

        }

        [Fact]
        public async Task GetPlanners_HavePageIndex_ListCountIsTwo_Test()
        {
            var sut = new GetPlannersListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannersListQuery
            {
                TeamId = twoPlanner_TeamId,
                PageIndex = 1
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);

        }
    }
}
