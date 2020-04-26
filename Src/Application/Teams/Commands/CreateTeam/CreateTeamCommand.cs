using ColabSpace.Application.Teams.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommand : IRequest<Guid>
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<UserModel> Users { get; set; }

        public string Description { get; set; }
    }
}
