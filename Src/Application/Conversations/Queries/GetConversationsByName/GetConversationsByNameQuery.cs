using ColabSpace.Application.Conversations.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsByName
{
    public class GetConversationsByNameQuery : IRequest<IEnumerable<ConversationModel>>
    {
        public string ConversationName { get; set; }
        public Guid LoginUserId { get; set; }
    }
}
