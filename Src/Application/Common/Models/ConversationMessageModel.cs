using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.MessageChats.Models;
using System.Collections.Generic;

namespace ColabSpace.Application.Common.Models
{
    public class ConversationMessageModel
    {
        public ConversationModel Conversation { get; set; }

        public ICollection<MessageChatModel> LstMessageChat { get; set; }
    }
}
