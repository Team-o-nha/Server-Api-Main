using AutoMapper;
using ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList;
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

namespace ColabSpace.Application.UnitTests.TaskItems.Queries.GetPinnedTaskItemList
{
    [Collection("QueryCollection")]
    public class GetPinnedTaskItemListQueryHandlerTests
    {
        private readonly Guid emptyPinnedTask_TeamId = ColabSpaceDbContextFactory.teamId2;
        private readonly Guid onePinnedTask_TeamId = ColabSpaceDbContextFactory.teamId4;
        private readonly Guid twoPinnedTask_TeamId = ColabSpaceDbContextFactory.teamId1;

        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetPinnedTaskItemListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPinnedTaskItems_ListIsEmpty_Test()
        {
            var sut = new GetPinnedTaskItemListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPinnedTaskItemListQuery
            {
                TeamId = emptyPinnedTask_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task GetPinnedTaskItems_ListIsOne_Test()
        {
            var sut = new GetPinnedTaskItemListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPinnedTaskItemListQuery
            {
                TeamId = onePinnedTask_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task GetPinnedTaskItems_ListIsTwo_Test()
        {
            var sut = new GetPinnedTaskItemListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPinnedTaskItemListQuery
            {
                TeamId = twoPinnedTask_TeamId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }
    }
}
