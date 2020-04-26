using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Notifications.Command;
using ColabSpace.Application.UnitTests.Common;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Notifications.Commands.ReadAndUnreadNotification
{
    public class ReadAndUnreadNotificationTests : CommandTestBase
    {
        private readonly Guid notificationIdUnread = ColabSpaceDbContextFactory.notificationId1;
        private readonly Guid notificationIdRead = ColabSpaceDbContextFactory.notificationId4;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        // ReadNotification
        [Fact]
        public async Task GiveInvalidNotificationId_ShouldRaiseNotFoundError()
        {
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());

            var sut = new ReadNotificationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new ReadNotificationCommand()
            {
                NotificationId = Guid.NewGuid()
            };

            await Should.ThrowAsync<NotFoundException>(() =>
               sut.Handle(command, CancellationToken.None));
        }

        // ReadNotification
        [Fact]
        public async Task GiveValidNotificationId_ShouldSetNotificationToReadStatus()
        {
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());

            var sut = new ReadNotificationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new ReadNotificationCommand()
            {
                NotificationId = notificationIdUnread
            };

            await sut.Handle(command, CancellationToken.None);

            var entity = _context.Notifications.Find(notificationIdUnread);

            entity.isRead.ShouldBeTrue();
        }

        // UnReadNotification
        [Fact]
        public async Task GiveInvalidNotificationId_ShouldRaiseNotFoundError2()
        {
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());

            var sut = new UnReadNotificationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new UnReadNotificationCommand()
            {
                NotificationId = Guid.NewGuid()
            };

            await Should.ThrowAsync<NotFoundException>(() =>
               sut.Handle(command, CancellationToken.None));
        }

        // UnReadNotification
        [Fact]
        public async Task GiveValidNotificationId_ShouldSetNotificationToUnreadStatus()
        {
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());

            var sut = new UnReadNotificationCommandHandler(_context, _currentUserServiceMock.Object);

            var command = new UnReadNotificationCommand()
            {
                NotificationId = notificationIdRead
            };

            await sut.Handle(command, CancellationToken.None);

            var entity = _context.Notifications.Find(notificationIdRead);

            entity.isRead.ShouldBeFalse();
        }
    }
}
