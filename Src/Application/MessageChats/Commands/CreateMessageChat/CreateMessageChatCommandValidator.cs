using FluentValidation;
using System;

namespace ColabSpace.Application.MessageChats.Commands.CreateMessageChat
{
    public class CreateMessageChatCommandValidator : AbstractValidator<CreateMessageChatCommand>
    {
        public CreateMessageChatCommandValidator()
        {
            RuleFor(v => v.Content).MaximumLength(5000);
            RuleFor(v => v.ConversationId).NotEqual(Guid.Empty);
            RuleFor(v => v.RegUserName).NotEmpty();
        }

    }
}
