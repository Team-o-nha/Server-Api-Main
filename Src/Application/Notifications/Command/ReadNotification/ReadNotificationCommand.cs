using MediatR;
using System;

namespace ColabSpace.Application.Notifications.Command
{
    public class ReadNotificationCommand : IRequest
    {
        public Guid NotificationId;
    }
}
