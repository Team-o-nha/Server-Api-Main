using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Commands.UpdateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.TaskItems.Queries.GetTaskItem;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Domain.Entities;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandHandlerTests : CommandTestBase
    {
        private IMapper _mapper;
        private readonly Guid validTaskItemId = ColabSpaceDbContextFactory.taskItemId1;
        private readonly Guid invalidTaskItemId = Guid.NewGuid();
        private readonly Guid memberTaskItemId = ColabSpaceDbContextFactory.taskItemId2;
        private readonly Guid validTeamId = ColabSpaceDbContextFactory.teamId1;
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
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = Guid.NewGuid();
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            var command = new UpdateTaskItemCommand
            {
                Id = invalidTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_GivenInvalidTeamId_ShouldRaiseNotFoundException()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = Guid.NewGuid();
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            var command = new UpdateTaskItemCommand
            {
                Id = validTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = Guid.NewGuid(),
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenInValidTeamIdRequest_ShouldRaiseNotFound()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = Guid.NewGuid();
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            var command = new UpdateTaskItemCommand
            {
                Id = validTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
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
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            _ = sut.Handle(new UpdateTaskItemCommand
            {
                Id = validTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
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

            result.Name.ShouldBe(taskName);
            result.Description.ShouldBe(description);
            result.Status.ShouldBe(1);
            result.TeamId.ShouldBe(teamId);
            result.Assignee.DisplayName.ShouldBe(user.DisplayName);
            result.Assignee.UserId.ShouldBe(user.UserId);
            result.CreatedBy.DisplayName.ShouldBe(user.DisplayName);
            result.CreatedBy.UserId.ShouldBe(user.UserId);
            result.AttachFiles.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateHistory()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
            currentUserServiceMock.Setup(m => m.UserName).Returns(userName1);
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };
            var relationsObject = new List<RelatedObjectModel>()
            {
                new RelatedObjectModel()
                {
                    ObjectId = Guid.NewGuid().ToString(),
                    Title = "Test Object",
                    Type = "Task",
                    Url = ""
                }
            };

            // Act
            _ = sut.Handle(new UpdateTaskItemCommand
            {
                Id = validTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
                Relations = relationsObject
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

            List<History> listHistories = result.Histories.ToList();

            result.Histories.ToList().Last().Title.ShouldBe("UserName1 has updated task: Name: \"Task1\" -> \"Task4\", Description: \"\" -> \"Description4\", Assignee: \"User3\" -> \"TestUser1\", Status: \"Unknown\" -> \"To Do\", AttachFiles, Relations");
            result.Histories.ToList().Last().Type.ShouldBe("Task");
            result.Histories.ToList().Last().Content.ShouldBe("Update");

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
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = userId1,
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            var command = new UpdateTaskItemCommand
            {
                Id = validTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
            };
            await Assert.ThrowsAsync<NotOwnedException>(() => sut.Handle(command, CancellationToken.None));

            // Assert
            //mediatorMock.Verify(m => m.Publish(It.Is<UpdateTaskItemCommand>(cc => cc.Id == validTaskItemId), It.IsAny<CancellationToken>()), Times.Once);
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
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            _ = sut.Handle(new UpdateTaskItemCommand
            {
                Id = memberTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
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

            result.Name.ShouldBe(taskName);
            result.Description.ShouldBe(description);
            result.Status.ShouldBe(1);
            result.TeamId.ShouldBe(teamId);
            result.Assignee.DisplayName.ShouldBe(user.DisplayName);
            result.Assignee.UserId.ShouldBe(user.UserId);
            result.CreatedBy.DisplayName.ShouldBe(user.DisplayName);
            result.CreatedBy.UserId.ShouldBe(user.UserId);
            result.AttachFiles.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_LoginUserAssignee_ShouldUpdateTaskItem()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId3.ToString());
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };

            // Act
            _ = sut.Handle(new UpdateTaskItemCommand
            {
                Id = memberTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
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

            result.Name.ShouldBe(taskName);
            result.Description.ShouldBe(description);
            result.Status.ShouldBe(1);
            result.TeamId.ShouldBe(teamId);
            result.Assignee.DisplayName.ShouldBe(user.DisplayName);
            result.Assignee.UserId.ShouldBe(user.UserId);
            result.CreatedBy.DisplayName.ShouldBe(user.DisplayName);
            result.CreatedBy.UserId.ShouldBe(user.UserId);
            result.AttachFiles.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_LoginUserAssignee_HaveTagsAndDeadline_ShouldUpdateTaskItem()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId3.ToString());
            var sut = new UpdateTaskItemCommandHandler(_context, mediatorMock.Object, currentUserServiceMock.Object);

            var taskName = "Task4";
            var description = "Description4";
            var teamId = validTeamId;
            var user = new UserModel()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "TestUser1"
            };
            var attachFiles = new List<AttachFileModel>()
            {
                new AttachFileModel()
                {
                    FileName = "file 1",
                }
            };
            var deadline = DateTime.Now;

            // Act
            _ = sut.Handle(new UpdateTaskItemCommand
            {
                Id = memberTaskItemId,
                Name = taskName,
                Description = description,
                Status = 1,
                TeamId = teamId,
                Assignee = user,
                CreatedBy = user,
                AttachFiles = attachFiles,
                Deadline = deadline,
                Tags = new List<TagModel>()
                {
                    new TagModel()
                    {
                        TagName = "abc"
                    }
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

            result.Name.ShouldBe(taskName);
            result.Description.ShouldBe(description);
            result.Status.ShouldBe(1);
            result.TeamId.ShouldBe(teamId);
            result.Assignee.DisplayName.ShouldBe(user.DisplayName);
            result.Assignee.UserId.ShouldBe(user.UserId);
            result.CreatedBy.DisplayName.ShouldBe(user.DisplayName);
            result.CreatedBy.UserId.ShouldBe(user.UserId);
            result.AttachFiles.Count.ShouldBe(1);
            result.Deadline.ShouldBe(deadline.ToUniversalTime());
            result.Tags.Count.ShouldBe(1);
        }
    }
}
