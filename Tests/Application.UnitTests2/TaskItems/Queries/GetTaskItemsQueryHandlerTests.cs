using AutoMapper;
using ColabSpace.Application.TaskItems.Queries.GetTaskItemsList;
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

namespace ColabSpace.Application.UnitTests2.TaskItems.Queries
{
    [Collection("QueryCollection")]
    public class GetTaskItemsQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid twoTask_TeamId = ColabSpaceDbContextFactory.teamId1;

        public GetTaskItemsQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task SearchTaskItem_EmptyList_Test()
        {
            var sut = new GetTaskItemsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemsQuery
            {
                TeamId = twoTask_TeamId,
                keyword = "invalid"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task SearchTaskItem_TaskName_Return_OneTask_Test()
        {
            var sut = new GetTaskItemsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemsQuery
            {
                TeamId = twoTask_TeamId,
                keyword = "Task1"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task SearchTaskItem_TaskName_Return_TwoTask_Test()
        {
            var sut = new GetTaskItemsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemsQuery
            {
                TeamId = twoTask_TeamId,
                keyword = "Task"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }

        [Fact]
        public async Task SearchTaskItem_Assignee_Return_OneTask_Test()
        {
            var sut = new GetTaskItemsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemsQuery
            {
                TeamId = twoTask_TeamId,
                keyword = "Assignee"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task SearchTaskItem_Assignee_Return_TwoTask_Test()
        {
            var sut = new GetTaskItemsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemsQuery
            {
                TeamId = twoTask_TeamId,
                keyword = "User"
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }
    }
}