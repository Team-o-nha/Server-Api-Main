using ColabSpace.Application.MessageChats.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Queries.GetMessageChatList
{
    public class GetMessageChatsQuery : IRequest<IEnumerable<MessageChatModel>>
    {
        public Guid ConversationId { get; set; }

        public int? PageIndex { get; set; }

        public Guid RelatedMessagesId { get; set; }

        public Guid? RelatedTaskId { get; set; }
    }
}
