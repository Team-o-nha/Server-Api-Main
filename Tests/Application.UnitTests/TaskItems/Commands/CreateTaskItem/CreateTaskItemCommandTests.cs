using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Application.TaskItems.Commands.CreateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Threading;
using ColabSpace.Application.Teams.Models;
using System.Collections.Generic;
using ColabSpace.Application.Common.Exceptions;
using Moq;
using ColabSpace.Application.Common.Interfaces;
using System.Linq;
using ColabSpace.Application.Common.Models;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandTests : CommandTestBase
    {
        private readonly Guid validTeamId = ColabSpaceDbContextFactory.teamId1;
        private readonly Guid validUserId = ColabSpaceDbContextFactory.userId1;
        private readonly Guid validUserId2 = ColabSpaceDbContextFactory.userId2;

        private CreateTaskItemCommandHandler SetLoginUser(Guid loginUserId)
        {
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(loginUserId.ToString());

            return new CreateTaskItemCommandHandler(_context, currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_GivenTeamIdInvalid_ShouldRaiseNotFoundException()
        {
            ////Arrange
            var user = new UserModel
            {
                DisplayName = "Valid User",
                UserId = validUserId
            };
            var sut = SetLoginUser(user.UserId);


            var command = new CreateTaskItemCommand
            {
                Name = "TestTask1",
                CreatedBy = user,
                Description = "Description",
                Assignee = user,
                Status = 1,
                TeamId = Guid.NewGuid(),
                AttachFiles = new List<AttachFileModel>()
                {
                    new AttachFileModel()
                    {
                        //Name = "fileName.txt",
                        //BinaryData=  "hjsad",
                        //Size = 4
                    }
                }
            };

            //// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldRaiseTaskItemCreatedNotificationAsync()
        {
            ////Arrange
            var user = new UserModel
            {
                DisplayName = "Valid User",
                UserId = validUserId
            };

            var sut = SetLoginUser(user.UserId);

            var command = new CreateTaskItemCommand
            {
                Name = "TestTask1",
                CreatedBy = user,
                Description = "Description",
                Assignee = user,
                Status = 1,
                TeamId = validTeamId,
                AttachFiles = new List<AttachFileModel>()
                {
                    new AttachFileModel()
                    {
                        //Name = "fileName.txt",
                        //BinaryData=  "hjsad",
                        //Size = 4
                    }
                }

            };

            //// Act
            var result = await sut.Handle(command, CancellationToken.None);

            var entity = _context.TaskItems.Find(result);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe("TestTask1");
            entity.Status.ShouldBe(1);
            entity.Assignee.DisplayName.ShouldBe("Valid User");
            entity.Description.ShouldBe("Description");
            entity.CreatedBy.DisplayName.ShouldBe("Valid User");
            entity.TeamId.ShouldBe(validTeamId);
            // Attach File test
            entity.AttachFiles.Count.ShouldBeGreaterThan(0);
            // Histories test
            entity.Histories.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldRaiseNotificationCreatedNotificationAsync()
        {
            ////Arrange
            var user = new UserModel
            {
                DisplayName = "Valid User",
                UserId = validUserId
            };

            var sut = SetLoginUser(validUserId2);

            var command = new CreateTaskItemCommand
            {
                Name = "TestTask1",
                CreatedBy = user,
                Description = "Description",
                Assignee = user,
                Status = 1,
                TeamId = validTeamId
            };

            //// Act
            var createdTaskId = await sut.Handle(command, CancellationToken.None);

            var entity = _context.Notifications.Where(notification => notification.TaskItemId == createdTaskId.ToString()).FirstOrDefault();
            entity.ShouldNotBeNull();
            entity.Type.ShouldBe("AddTask");
        }

        [Fact]
        public async Task Handle_GivenValidRequestHaveTagAndDeadline_ShouldRaiseTaskItemCreatedNotificationAsync()
        {
            ////Arrange
            var user = new UserModel
            {
                DisplayName = "Valid User",
                UserId = validUserId
            };

            var sut = SetLoginUser(user.UserId);
            var deadline = DateTime.Now;

            var command = new CreateTaskItemCommand
            {
                Name = "TestTask1",
                CreatedBy = user,
                Description = "Description",
                Assignee = user,
                Status = 1,
                TeamId = validTeamId,
                AttachFiles = new List<AttachFileModel>()
                {
                    new AttachFileModel()
                    {
                        //Name = "fileName.txt",
                        //BinaryData=  "hjsad",
                        //Size = 4
                    }
                },
                Deadline = deadline,
                Tags = new List<TagModel>()
                {
                    new TagModel()
                    {
                        TagName = "abc"
                    }
                }
            };

            //// Act
            var result = await sut.Handle(command, CancellationToken.None);

            var entity = _context.TaskItems.Find(result);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe("TestTask1");
            entity.Status.ShouldBe(1);
            entity.Assignee.DisplayName.ShouldBe("Valid User");
            entity.Description.ShouldBe("Description");
            entity.CreatedBy.DisplayName.ShouldBe("Valid User");
            entity.TeamId.ShouldBe(validTeamId);
            // Attach File test
            entity.AttachFiles.Count.ShouldBeGreaterThan(0);
            entity.Deadline.ShouldBe(deadline.ToUniversalTime());
            entity.Tags.Count.ShouldBe(1);
            // Histories test
            entity.Histories.Count.ShouldBe(1);
        }
    }
}

