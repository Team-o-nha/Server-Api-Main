using ColabSpace.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class MessageChat : AuditableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public Guid ConversationId { get; set; }

        public string ConversationName { get; set; }

        public string RegUserName { get; set; }

        public ICollection<AttachFile> AttachFileList { get; set; }

        public ICollection<Reaction> ReactionList { get; set; }

        public bool IsPin { get; set; }

        public DateTime PinnedDate { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public Guid? RelatedTaskId { get; set; }

        public MessageChat RelatedMessages { get; set; }
    }
}
