using ColabSpace.Application.Teams.Models;
using MediatR;
using System.Collections.Generic;

namespace ColabSpace.Application.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommand : IRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserModel> Users { get; set; }
    }
}
