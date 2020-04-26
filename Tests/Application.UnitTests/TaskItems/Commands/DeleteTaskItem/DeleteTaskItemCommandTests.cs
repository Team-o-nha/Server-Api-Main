using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.TaskItems.Commands.DeleteTaskItem;
using ColabSpace.Application.UnitTests.Common;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandTests : CommandTestBase
    {
        private DeleteTaskItemCommandHandler _sut;
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }).CreateMapper();
        private readonly Guid taskItemId = ColabSpaceDbContextFactory.taskItemId2;
        private readonly Guid teamNotExistTaskItemId = ColabSpaceDbContextFactory.taskItemId4;
        public static Guid leaderId = ColabSpaceDbContextFactory.userId1;
        public static Guid creatorId = ColabSpaceDbContextFactory.userId2;
        public static Guid assigneeId = ColabSpaceDbContextFactory.userId3;
        public static Guid memberId = ColabSpaceDbContextFactory.userId4;

        public DeleteTaskItemCommandTests()
            : base()
        {
        }

        private DeleteTaskItemCommandHandler SetLoginUser(Guid loginUserId)
        {
            // Login user
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns(loginUserId.ToString());

            return new DeleteTaskItemCommandHandler(_context, currentUserServiceMock.Object, _mapper);
        }

        /**
         * Given task item id equals null throws ArgumentNullException
         */
        [Fact]
        public async Task Handle_GivenNullTaskItemId_ThrowsArgumentNullException()
        {
            // Login user is creator
            _sut = SetLoginUser(creatorId);

            var command = new DeleteTaskItemCommand { Id = null };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given task item does not exist throws NotFoundException
         */
        [Fact]
        public async Task Handle_GivenInvalidTaskItemId_ThrowsNotFoundException()
        {
            // Login user is creator
            _sut = SetLoginUser(creatorId);

            var invalidTaskId = Guid.NewGuid();

            var command = new DeleteTaskItemCommand { Id = invalidTaskId.ToString() };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given task item exist, team does not exist
         * Login user is not creator
         * throws NotFoundException
         */
        [Fact]
        public async Task Handle_GivenValidTaskItemId_LoginUserIsNotCreatorAndTeamIdNotExist_ThrowsNotFoundException()
        {
            // Login user is not creator
            _sut = SetLoginUser(memberId);

            var command = new DeleteTaskItemCommand { Id = teamNotExistTaskItemId.ToString() };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given: task item exist, team exist
         * Login user is not creator, is not assignee and is not leader
         * throws NotOwnedException
         */
        [Fact]
        public async Task Handle_GivenValidTaskItemIdAndValidTeamId_LoginUserIsNotCreatorAndNotLeaderAndNotAssignee_ThrowsNotOwnedException()
        {
            // Login user is member but is not creator and is not assignee
            _sut = SetLoginUser(memberId);

            var command = new DeleteTaskItemCommand { Id = taskItemId.ToString() };

            await Assert.ThrowsAsync<NotOwnedException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given: task item exist, team exist
         * Login user is not creator but is leader
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidTaskItemIdAndValidTeamId_LoginUserIsLeader_DeletesTaskItem()
        {
            // Login user is leader
            _sut = SetLoginUser(leaderId);

            var command = new DeleteTaskItemCommand { Id = taskItemId.ToString() };

            await _sut.Handle(command, CancellationToken.None);

            var taskItem = await _context.TaskItems.FindAsync(taskItemId);

            Assert.Null(taskItem);
        }

        /**
         * Given task item exist, team exist
         * Login user is not creator, is not leader but is assignee
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidTaskItemIdAndValidTeamId_LoginUserIsAssignee_DeletesTaskItem()
        {
            // Login user is assignee
            _sut = SetLoginUser(assigneeId);

            var command = new DeleteTaskItemCommand { Id = taskItemId.ToString() };

            await _sut.Handle(command, CancellationToken.None);

            var taskItem = await _context.TaskItems.FindAsync(taskItemId);

            Assert.Null(taskItem);
        }

        /**
         * Given task item exist, team exist
         * Login user is creator
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidTaskItemIdAndValidTeamId_LoginUserIsCreator_DeletesTaskItem()
        {
            // Login user is creator
            _sut = SetLoginUser(creatorId);

            var command = new DeleteTaskItemCommand { Id = taskItemId.ToString() };

            await _sut.Handle(command, CancellationToken.None);

            var taskItem = await _context.TaskItems.FindAsync(taskItemId);

            Assert.Null(taskItem);
        }
    }
}
