using ColabSpace.Application.Notifications.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Notifications.Queries.GetNotificationsByTeam
{
    public class GetNotificationsByTeamQuery : IRequest<IEnumerable<NotificationByTeamModel>>
    {
        public Guid UserId { get; set; }
    }
}
