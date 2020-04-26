using FluentValidation;

namespace ColabSpace.Application.Conversations.Commands.UpdateConversationName
{
    public class UpdateConversationNameCommandValidator : AbstractValidator<UpdateConversationNameCommand>
    {
        public UpdateConversationNameCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200);
        }
    }
}
