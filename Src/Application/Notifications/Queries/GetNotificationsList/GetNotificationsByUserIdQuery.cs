using ColabSpace.Application.Notifications.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Notifications.Queries.GetNotificationsList
{
    public class GetNotificationsByUserIdQuery : IRequest<NotificationsPagingModel>
    {
        public Guid UserId { get; set; }

        public string TeamId { get; set; }

        public int? PageIndex { get; set; }
    }
}