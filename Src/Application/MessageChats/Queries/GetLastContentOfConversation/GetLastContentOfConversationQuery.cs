using ColabSpace.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.MessageChats.Queries.GetLastContentOfConversation
{
    public class GetLastContentOfConversationQuery : IRequest<IEnumerable<ConversationLastContentModel>>
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
