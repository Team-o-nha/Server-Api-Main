using ColabSpace.Application.Planners.Models;
using MediatR;
using System;

namespace ColabSpace.Application.Planners.Queries.GetPlanner
{
    public class GetPlannerQuery : IRequest<PlannerModel>
    {
        public Guid PlannerId { get; set; }
    }
}
