using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Common.Models;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Planners.Models
{
    public class PlannerModel : IMapFrom<Planner>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public string Purpose { get; set; }

        public ICollection<MilestoneModel> Milestones { get; set; }

        public Guid? TeamId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
