using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using System;
using System.Collections.Generic;

namespace ColabSpace.WebAPI.Models
{
    public class MessageDto
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public Guid ConversationId { get; set; }
        public ICollection<UserModel> Mentions { get; set; }

        public ICollection<AttachFileModel> AttachFiles { get; set; }

        public ICollection<ReactionModel> Reactions { get; set; }

        public bool IsPin { get; set; }

        public string TeamId { get; set; }

        public string ConversationName { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public Guid? RelatedTaskId { get; set; }

    }
}
