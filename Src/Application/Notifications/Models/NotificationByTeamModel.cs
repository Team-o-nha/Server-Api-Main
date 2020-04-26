using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Notifications.Models
{
    public class NotificationByTeamModel
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public ICollection<NotificationModel> Notifications { get; set; }
    }
}
