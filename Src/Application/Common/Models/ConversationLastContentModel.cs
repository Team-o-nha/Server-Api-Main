using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.MessageChats.Models;

namespace ColabSpace.Application.Common.Models
{
    public class ConversationLastContentModel
    {
        public ConversationModel Conversation { get; set; }

        public MessageChatModel LastMessageChatContent { get; set; }

        public int UnreadCounter { get; set; }
    }
}
