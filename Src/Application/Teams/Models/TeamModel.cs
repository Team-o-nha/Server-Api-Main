using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Teams.Models
{
    public class TeamModel : IMapFrom<Team>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserModel> Users { get; set; }

        public string Description { get; set; }
    }
}