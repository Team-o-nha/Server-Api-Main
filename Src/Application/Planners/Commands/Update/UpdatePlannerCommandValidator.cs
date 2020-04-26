using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Planners.Commands.Update
{
    public class UpdatePlannerCommandValidator : AbstractValidator<UpdatePlannerCommand>
    {
        public UpdatePlannerCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Title).MaximumLength(200).NotEmpty();
        }
    }
}
