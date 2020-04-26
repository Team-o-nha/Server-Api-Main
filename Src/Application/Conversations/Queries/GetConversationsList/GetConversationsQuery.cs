using ColabSpace.Application.Conversations.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsList
{
    public class GetConversationsQuery : IRequest<IEnumerable<ConversationModel>>
    {
        public Guid UserId { get; set; }
        public String ConversationName { get; set; }
        public Guid TeamId { get; set; }
    }
}
