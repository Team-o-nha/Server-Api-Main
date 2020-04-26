using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Planners.Commands
{
    public class CreatePlannerCommand : IRequest<PlannerModel>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public string Purpose { get; set; }
        public Guid TeamId { get; set; }

    }
}
