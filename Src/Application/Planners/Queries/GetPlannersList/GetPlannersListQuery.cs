using ColabSpace.Application.Planners.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Planners.Queries.GetPlannersList
{
    public class GetPlannersListQuery : IRequest<IEnumerable<PlannerModel>>
    {
        public Guid TeamId { get; set; }

        public int? PageIndex { get; set; }

        public string Keyword { get; set; }
    }
}
