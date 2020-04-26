using ColabSpace.Application.Teams.Models;
using MediatR;
using System;

namespace ColabSpace.Application.Teams.Queries.GetTeam
{
    public class GetTeamQuery : IRequest<TeamModel>
    {
        public Guid TeamId { get; set; }
    }
}
