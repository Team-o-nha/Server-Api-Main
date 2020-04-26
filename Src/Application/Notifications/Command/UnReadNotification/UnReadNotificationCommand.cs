using MediatR;
using System;

namespace ColabSpace.Application.Notifications.Command
{
    public class UnReadNotificationCommand : IRequest
    {
        public Guid NotificationId;
    }
}
