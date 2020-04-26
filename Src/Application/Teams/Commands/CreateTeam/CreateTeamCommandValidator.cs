using FluentValidation;

namespace ColabSpace.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        }
    }
}
