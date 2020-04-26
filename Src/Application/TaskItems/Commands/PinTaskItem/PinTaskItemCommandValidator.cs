using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.TaskItems.Commands.PinTaskItem
{
    public class PinTaskItemCommandValidator : AbstractValidator<PinTaskItemCommand>
    {
        public PinTaskItemCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty);
        }
    }
}
