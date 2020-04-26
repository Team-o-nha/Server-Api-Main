using ColabSpace.Application.Conversations.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsListByTeamId
{
    public class GetConversationsByTeamIdQuery : IRequest<IEnumerable<ConversationModel>>
    {
        public Guid TeamId { get; set; }
    }
}
