using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.TaskItems.Queries.GetTaskItem;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Queries.GetTaskItem
{
    [Collection("QueryCollection")]
    public class GetTaskItemQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validTaskId = ColabSpaceDbContextFactory.taskItemId1;
        private readonly Guid invalidTaskId = Guid.NewGuid();

        public GetTaskItemQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetTaskItem_NotFound_Test()
        {
            var sut = new GetTaskItemQueryHandler(_context, _mapper);

            var command = new GetTaskItemQuery
            {
                TaskItemId = invalidTaskId
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task GetTaskItem_Found_Test()
        {
            var sut = new GetTaskItemQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetTaskItemQuery
            {
                TaskItemId = validTaskId
            }
            , CancellationToken.None);

            result.ShouldBeOfType<TaskItemModel>();
            result.Id.ShouldBe(validTaskId);
        }
    }
}
