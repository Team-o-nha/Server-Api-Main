using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Planners.Queries.GetPlanner
{
    public class GetPlannerQueryHandler : IRequestHandler<GetPlannerQuery, PlannerModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetPlannerQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlannerModel> Handle(GetPlannerQuery request, CancellationToken cancellationToken)
        {
            var planner = await _context.Planners.FindAsync(request.PlannerId);
            if (planner == null)
            {
                throw new NotFoundException(nameof(Planner), request.PlannerId);
            }
            var team = await _context.Teams.FindAsync(planner.TeamId);
            var leader = team.Users.Where(u => u.TeamRole == "Leader").FirstOrDefault();
            var plannerModel = _mapper.Map<PlannerModel>(planner);

            var milestones = new List<MilestoneModel>();
            foreach (var milestone in plannerModel.Milestones)
            {
                milestone.Tasks = new List<TaskItemModel>();
                foreach (var taskId in milestone.TaskIds)
                {
                    var taskItem = await _context.TaskItems.FindAsync(taskId);
                    if (taskItem != null)
                    {
                        taskItem.Leader = leader;
                        milestone.Tasks.Add(_mapper.Map<TaskItemModel>(taskItem));
                    }
                }
                milestones.Add(milestone);
            }
            plannerModel.Milestones = milestones;
            return plannerModel;
        }
    }
}
