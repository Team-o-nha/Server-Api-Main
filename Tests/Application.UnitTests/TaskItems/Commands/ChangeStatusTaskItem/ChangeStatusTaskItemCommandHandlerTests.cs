using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.TaskItems.Commands.ChangeStatusTaskItem;
using ColabSpace.Application.TaskItems.Queries.GetTaskItem;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.ChangeStatusTaskItem
{
    public class ChangeStatusTaskItemCommandHandlerTests : CommandTestBase
    {

        private IMapper _mapper;
        private readonly Guid validTaskItemId = ColabSpaceDbContextFactory.taskItemId1;
        private readonly Guid invalidTaskItemId = Guid.NewGuid();
        private readonly Guid memberTaskItemId = ColabSpaceDbContextFactory.taskItemId2;
        private static Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private static Guid userId2 = ColabSpaceDbContextFactory.userId2;
        private static Guid userId3 = ColabSpaceDbContextFactory.userId3;
        private static string userName1 = "UserName1";

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldRaiseException()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            var command = new ChangeStatusTaskItemCommand
            {
                Id = invalidTaskItemId,
                Status = 1,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldUpdateTaskItem()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            _ = sut.Handle(new ChangeStatusTaskItemCommand
            {
                Id = validTaskItemId,
                Status = 3,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            }, CancellationToken.None);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            var result = await new GetTaskItemQueryHandler(_context, _mapper).Handle(new GetTaskItemQuery
            {
                TaskItemId = validTaskItemId
            }, CancellationToken.None);

            result.Status.ShouldBe(3);
            result.Assignee.UserId.ShouldBe(assigneeId);
            result.Assignee.DisplayName.ShouldBe("AssigneeTest");

            // Assert
            //mediatorMock.Verify(m => m.Publish(It.Is<ChangeStatusTaskItemCommand>(cc => cc.Id == validTaskItemId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_LoginUserIsNotOwner_ShouldRaiseNotOwned()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId2.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();

            // Act
            var command = new ChangeStatusTaskItemCommand
            {
                Id = validTaskItemId,
                Status = 1,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            };
            await Assert.ThrowsAsync<NotOwnedException>(() => sut.Handle(command, CancellationToken.None));

            // Assert
            //mediatorMock.Verify(m => m.Publish(It.Is<ChangeStatusTaskItemCommand>(cc => cc.Id == validTaskItemId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_LoginUserIsLeader_ShouldUpdateTaskItem()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            _ = sut.Handle(new ChangeStatusTaskItemCommand
            {
                Id = memberTaskItemId,
                Status = 3,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            }, CancellationToken.None);


            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            var result = await new GetTaskItemQueryHandler(_context, _mapper).Handle(new GetTaskItemQuery
            {
                TaskItemId = memberTaskItemId
            }, CancellationToken.None);

            result.Status.ShouldBe(3);
            result.Assignee.UserId.ShouldBe(assigneeId);
            result.Assignee.DisplayName.ShouldBe("AssigneeTest");
        }

        [Fact]
        public async Task Handle_LoginUserIsAssignee_ShouldUpdateTaskItem()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId3.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            _ = sut.Handle(new ChangeStatusTaskItemCommand
            {
                Id = memberTaskItemId,
                Status = 3,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            }, CancellationToken.None);


            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            var result = await new GetTaskItemQueryHandler(_context, _mapper).Handle(new GetTaskItemQuery
            {
                TaskItemId = memberTaskItemId
            }, CancellationToken.None);

            result.Status.ShouldBe(3);
            result.Assignee.UserId.ShouldBe(assigneeId);
            result.Assignee.DisplayName.ShouldBe("AssigneeTest");
        }

        [Fact]
        public async Task Handle_LoginUserIsTaskCreater_ShouldCreateNotification()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId3.ToString());
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            await sut.Handle(new ChangeStatusTaskItemCommand
            {
                Id = memberTaskItemId,
                Status = 3,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            }, CancellationToken.None);

            var entity = _context.Notifications.Where(notification => notification.TaskItemId == memberTaskItemId.ToString()).FirstOrDefault();
            entity.ShouldNotBeNull();
            entity.Type.ShouldBe("DoneTask");
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldUpdateTaskItemAndCreateHistory()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            currentUserServiceMock.Setup(m => m.UserName).Returns(userName1);
            var sut = new ChangeStatusTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var assigneeId = Guid.NewGuid();
            // Act
            _ = sut.Handle(new ChangeStatusTaskItemCommand
            {
                Id = validTaskItemId,
                Status = 3,
                Assignee = new UserModel
                {
                    UserId = assigneeId,
                    DisplayName = "AssigneeTest"
                }
            }, CancellationToken.None);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            var result = await new GetTaskItemQueryHandler(_context, _mapper).Handle(new GetTaskItemQuery
            {
                TaskItemId = validTaskItemId
            }, CancellationToken.None);

            result.Status.ShouldBe(3);
            result.Assignee.UserId.ShouldBe(assigneeId);
            result.Assignee.DisplayName.ShouldBe("AssigneeTest");
            result.Histories.Last().Title.ShouldBe("UserName1 has updated task: Status: \"Unknown\" -> \"Done\", Assignee: \"User3\" -> \"AssigneeTest\"");

            // Assert
            //mediatorMock.Verify(m => m.Publish(It.Is<ChangeStatusTaskItemCommand>(cc => cc.Id == validTaskItemId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
