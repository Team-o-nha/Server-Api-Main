using FluentValidation;
using System;

namespace ColabSpace.Application.Planners.Commands
{
    public class CreatePlannerCommandValidator : AbstractValidator<CreatePlannerCommand>
    {
        public CreatePlannerCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Title).MaximumLength(200).NotEmpty();
        }
    }
}
