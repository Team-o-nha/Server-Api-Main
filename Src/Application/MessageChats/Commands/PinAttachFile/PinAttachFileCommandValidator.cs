using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Commands.PinAttachFile
{
    public class PinAttachFileCommandValidator : AbstractValidator<PinAttachFileCommand>
    {
        public PinAttachFileCommandValidator()
        {
            RuleFor(command => command.MessageId).NotEqual(Guid.Empty);
            RuleFor(command => command.BlobStorageUrl).NotNull().NotEmpty();
        }
    }
}
