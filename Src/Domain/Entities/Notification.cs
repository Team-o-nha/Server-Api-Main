using ColabSpace.Domain.Common;
using Newtonsoft.Json;
using System;

namespace ColabSpace.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        [JsonProperty(PropertyName = "id")]
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
        public string RegUserId { get; set; }
        public User ToUser { get; set; }
        public bool isRead { get; set; }
        public string URL { get; set; }
        public string TaskItemId { get; set; }
    }
}
