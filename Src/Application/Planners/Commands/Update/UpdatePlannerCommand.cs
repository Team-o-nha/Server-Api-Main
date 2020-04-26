using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Planners.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Planners.Commands.Update
{
    public class UpdatePlannerCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public string Purpose { get; set; }

        public ICollection<MilestoneModel> Milestones { get; set; }
    }
}
