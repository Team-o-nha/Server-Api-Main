using MediatR;

namespace ColabSpace.Application.Teams.Commands.DeleteTeam
{
    public class DeleteTeamCommand : IRequest
    {
        public string Id { get; set; }
    }
}
