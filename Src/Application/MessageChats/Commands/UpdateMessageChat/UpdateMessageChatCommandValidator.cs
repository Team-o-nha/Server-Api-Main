using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Commands.UpdateMessageChat
{
    public class UpdateMessageChatCommandValidator : AbstractValidator<UpdateMessageChatCommand>
    {
        public UpdateMessageChatCommandValidator()
        {
            RuleFor(v => v.MessageId).NotEqual(Guid.Empty);
        }
    }
}
