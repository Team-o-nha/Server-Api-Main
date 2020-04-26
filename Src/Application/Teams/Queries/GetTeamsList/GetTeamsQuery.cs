using ColabSpace.Application.Teams.Models;
using MediatR;
using System.Collections.Generic;

namespace ColabSpace.Application.Teams.Queries.GetTeamsList
{
    public class GetTeamsQuery : IRequest<IEnumerable<TeamModel>>
    {
        public string UserId { get; set; }

        public string TeamName { get; set; }
    }
}
