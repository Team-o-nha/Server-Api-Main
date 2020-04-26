using FluentValidation;

namespace ColabSpace.Application.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200);
            RuleFor(v => v.Type).NotEmpty();
            RuleFor(v => v.Members).Must(list => list.Count >= 2);
        }

    }
}
