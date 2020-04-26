using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Planners.Commands.Update;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.Planners.Queries.GetPlanner;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.UnitTests.Common;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Commands
{
    public class UpdatePlannerCommandHandlerTest : CommandTestBase
    {
        private readonly Guid validPlannerId = ColabSpaceDbContextFactory.plannerId1;
        private readonly Guid validTaskId1 = ColabSpaceDbContextFactory.taskItemId1;
        private readonly Guid validTaskId2 = ColabSpaceDbContextFactory.taskItemId1;
        private readonly Guid invalidPlannerId = Guid.NewGuid();

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldUpdatePlanner()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var sut = new UpdatePlannerCommandHandler(_context, mediatorMock.Object, _mapper);

            // data to update
            List<MilestoneModel> listMilestone = new List<MilestoneModel>();
            List<TaskItemModel> listTask = new List<TaskItemModel>();
            List<TagModel> listTag = new List<TagModel>();

            listTask.AddRange(new[] {
                new TaskItemModel
                {
                    Id = validTaskId1
                },
                new TaskItemModel
                {
                    Id = validTaskId2
                }
            });

            listMilestone.AddRange(new[] {
                new MilestoneModel
                {
                    Title = "Updated Milestone 1",
                    Description = "Updated Milestone 1 Description",
                    Tasks = listTask,
                    Date = DateTime.UtcNow,
                }
            });

            listTag.AddRange(new[] {
                new TagModel
                {
                    TagName = "Tag 1"
                },
                new TagModel
                {
                    TagName = "Tag 2"
                }
            });

            // Act
            _ = sut.Handle(new UpdatePlannerCommand
            {
                Id = validPlannerId,
                Title = "Updated Title",
                Purpose = "Updated Purpose",
                Milestones = listMilestone,
                Tags = listTag,
            }, CancellationToken.None);

            var result = await _context.Planners.FindAsync(validPlannerId);

            result.Title.ShouldBe("Updated Title");
            result.Purpose.ShouldBe("Updated Purpose");
            result.Milestones.Count.ShouldBe(1);
            result.Tags.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldRaiseNotFound()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var sut = new UpdatePlannerCommandHandler(_context, mediatorMock.Object, _mapper);

            // data to update
            List<MilestoneModel> listMilestone = new List<MilestoneModel>();
            List<TaskItemModel> listTask = new List<TaskItemModel>();
            List<TagModel> listTag = new List<TagModel>();

            listTask.AddRange(new[] {
                new TaskItemModel
                {
                    Id = validTaskId1
                },
                new TaskItemModel
                {
                    Id = validTaskId2
                }
            });

            listMilestone.AddRange(new[] {
                new MilestoneModel
                {
                    Title = "Updated Milestone 1",
                    Description = "Updated Milestone 1 Description",
                    Tasks = listTask,
                    Date = DateTime.UtcNow,
                }
            });

            listTag.AddRange(new[] {
                new TagModel
                {
                    TagName = "Tag 1"
                },
                new TagModel
                {
                    TagName = "Tag 2"
                }
            });

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(new UpdatePlannerCommand
            { Id = invalidPlannerId }, CancellationToken.None));
        }
    }
}
