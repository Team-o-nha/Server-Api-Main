using FluentValidation;

namespace ColabSpace.Application.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        }
    }
}
