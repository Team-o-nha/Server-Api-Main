using ColabSpace.Application.Conversations.Models;
using MediatR;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Queries.GetConversation
{
    public class GetConversationQuery : IRequest<ConversationModel>
    {
        public ICollection<string> MembersId { get; set; }
        public string ConversationId { get; set; }
        public string LoginUserId { get; set; }
    }
}
