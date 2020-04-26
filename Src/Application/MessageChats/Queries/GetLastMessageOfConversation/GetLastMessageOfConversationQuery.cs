using ColabSpace.Application.MessageChats.Models;
using MediatR;
using System;

namespace ColabSpace.Application.MessageChats.Queries.GetLastMessageOfConversation
{
    public class GetLastMessageOfConversationQuery : IRequest<MessageChatModel>
    {
        public Guid ConversationId { get; set; }
    }
}
