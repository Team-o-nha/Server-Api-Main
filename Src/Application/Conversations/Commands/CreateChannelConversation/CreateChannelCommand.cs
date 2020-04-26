using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Teams.Models;
using MediatR;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Commands.CreateChannelConversation
{
    public class CreateChannelConversationCommand : IRequest<ConversationModel>
    {
        public string Name { get; set; }
        public string ChannelDescription { get; set; }
        public ICollection<UserModel> Members { get; set; }
        public string TeamId { get; set; }
        public bool IsPublic { get; set; }
    }
}
