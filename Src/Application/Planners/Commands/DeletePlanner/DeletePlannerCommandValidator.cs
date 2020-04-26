using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandValidator : AbstractValidator<DeletePlannerCommand>
    {
        public DeletePlannerCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}
