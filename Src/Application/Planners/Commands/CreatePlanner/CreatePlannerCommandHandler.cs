using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Planners.Commands
{
    public class CreatePlannerCommandHandler : IRequestHandler<CreatePlannerCommand, PlannerModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public CreatePlannerCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PlannerModel> Handle(CreatePlannerCommand request, CancellationToken cancellationToken)
        {
            var tags = new List<Tag>();
            if (request.Tags != null)
            {
                foreach (var tag in request.Tags)
                {
                    tags.Add(new Tag() { TagName = tag.TagName });
                }
            }
            Planner planner = new Planner()
            {
                Id = request.Id,
                Title = request.Title,
                Purpose = request.Purpose,
                Tags = tags,
                TeamId = request.TeamId,
                Milestones = new List<Milestone>()
            };
            _context.Planners.Add(planner);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PlannerModel>(planner);
        }
    }
}
