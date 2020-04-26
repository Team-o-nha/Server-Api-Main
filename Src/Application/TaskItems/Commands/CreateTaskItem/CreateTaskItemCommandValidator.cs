using FluentValidation;

namespace ColabSpace.Application.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
    {
        public CreateTaskItemCommandValidator()
        {
            RuleFor(x => x.Name).MaximumLength(200).NotEmpty();
            //RuleFor(x => x.Content).MaximumLength(60).NotEmpty();
        }
    }
}
