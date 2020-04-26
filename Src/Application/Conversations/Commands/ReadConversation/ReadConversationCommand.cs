using MediatR;

namespace ColabSpace.Application.Conversations.Commands.ReadConversation
{
    public class ReadConversationCommand : IRequest
    {
        public string ConversationId;
    }
}
