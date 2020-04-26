using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.Planners.Queries.GetPlanner;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Queries.GetPlanner
{
    [Collection("QueryCollection")]
    public class GetPlannerQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        private readonly Guid emptyPlannerId = Guid.Empty;
        private readonly Guid validPlannerId = ColabSpaceDbContextFactory.plannerId1;

        public GetPlannerQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPlanners_EmptyPlannerId_Test()
        {
            var sut = new GetPlannerQueryHandler(_context, _mapper);

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(new GetPlannerQuery
            {
                PlannerId = emptyPlannerId
            }, CancellationToken.None));
        }

        [Fact]
        public async Task GetPlanners_ValidPlannerId_Test()
        {
            var sut = new GetPlannerQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPlannerQuery
            {
                PlannerId = validPlannerId
            }
            , CancellationToken.None);

            var entity = _context.Planners.Find(validPlannerId);

            result.Title.ShouldBe(entity.Title);
            result.TeamId.ShouldBe(entity.TeamId);
            result.Tags.Count.ShouldBe(entity.Tags.Count);
            result.Purpose.ShouldBe(entity.Purpose);

            foreach (var tag in result.Tags)
            {
                entity.Tags.Any(t => t.TagName == tag.TagName).ShouldBeTrue();
            }

            foreach (MilestoneModel milestone in result.Milestones.ToList())
            {
                entity.Milestones.Any(t => t.Title == milestone.Title).ShouldBeTrue();
                entity.Milestones.Any(t => t.Date == milestone.Date).ShouldBeTrue();
                entity.Milestones.Any(t => t.Description == milestone.Description).ShouldBeTrue();

                milestone.Tasks.ToList().ForEach(taskResult =>
                {
                    var task = _context.TaskItems.Find(taskResult.Id);
                    taskResult.Name.ShouldBe(task.Name);
                    taskResult.Status.ShouldBe(task.Status);
                    taskResult.Id.ShouldBe(task.Id);
                    taskResult.TeamId.ShouldBe(task.TeamId ?? Guid.Empty);
                });

            }
        }
    }
}
