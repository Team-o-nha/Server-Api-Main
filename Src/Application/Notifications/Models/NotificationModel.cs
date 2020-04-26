using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using System;

namespace ColabSpace.Application.Notifications.Models
{
    public class NotificationModel : IMapFrom<Notification>
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string ConversationId { get; set; }
        public string ConversationName { get; set; }
        public string MessageId { get; set; }
        public string MessageContent { get; set; }
        public string RegUserName { get; set; }
        public UserModel ToUser { get; set; }
        public bool isRead { get; set; }
        public string URL { get; set; }
        public DateTime Created { get; set; }
        public string TaskItemId { get; set; }
    }
}
