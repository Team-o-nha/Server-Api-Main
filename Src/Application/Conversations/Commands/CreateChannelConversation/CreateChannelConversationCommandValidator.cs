using ColabSpace.Application.Conversations.Commands.CreateChannelConversation;
using FluentValidation;

namespace ColabSpace.Application.Conversations.Commands.CreateConversation
{
    public class CreateChannelConversationCommandValidator : AbstractValidator<CreateChannelConversationCommand>
    {
        public CreateChannelConversationCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
            RuleFor(v => v.Members).NotEmpty().When(v => v.IsPublic = false);
            RuleFor(v => v.TeamId).NotEmpty();
            RuleFor(v => v.ChannelDescription).MaximumLength(200);
        }
    }
}
