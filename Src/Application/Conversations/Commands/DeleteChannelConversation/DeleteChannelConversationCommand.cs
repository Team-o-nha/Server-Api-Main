using MediatR;

namespace ColabSpace.Application.Conversations.Commands.DeleteChannelConversation
{
    public class DeleteChannelConversationCommand : IRequest
    {
        public string ConversationId { get; set; }
    }
}
