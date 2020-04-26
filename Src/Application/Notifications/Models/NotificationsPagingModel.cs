using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Notifications.Models
{
    public class NotificationsPagingModel
    {
        public int UnreadFeedCount { get; set; }
        public ICollection<NotificationModel> Notifications { get; set; }
    }
}
