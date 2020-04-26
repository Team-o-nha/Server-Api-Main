using FluentValidation;

namespace ColabSpace.Application.Conversations.Commands.DeleteChannelConversation
{
    public class DeleteChannelConversationCommandValidator : AbstractValidator<DeleteChannelConversationCommand>
    {

        public DeleteChannelConversationCommandValidator()
        {
            RuleFor(cmd => cmd.ConversationId).NotEmpty();
        }
    }
}
