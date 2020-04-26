using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Planners.Commands.Update
{
    public class UpdatePlannerCommandHandler : IRequestHandler<UpdatePlannerCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdatePlannerCommandHandler(IColabSpaceDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdatePlannerCommand request, CancellationToken cancellationToken)
        {
            // For test
            var planner = await _context.Planners.FindAsync(request.Id);
            if (planner == null)
            {
                throw new NotFoundException(nameof(Planner), request.Id);
            }
            // update Tags
            List<Tag> tags = new List<Tag>();
            if (request.Tags != null)
            {
                foreach (TagModel tag in request.Tags)
                {
                    tags.Add(new Tag()
                    {
                        TagName = tag.TagName
                    });
                }
            }
            //update milestones
            List<Milestone> milestones = new List<Milestone>();
            if (request.Milestones != null)
            {
                foreach (var m in request.Milestones)
                {
                    List<Guid> listTaskId = new List<Guid>();
                    if (m.Tasks != null)
                    {
                        foreach (var task in m.Tasks)
                        {
                            listTaskId.Add(task.Id);
                        }
                    }
                    milestones.Add(new Milestone()
                    {
                        Title = m.Title,
                        Date = m.Date,
                        Description = m.Description,
                        TaskIds = listTaskId,
                    });
                }
            }
            // update operation
            planner.Tags = tags;
            planner.Milestones = milestones;
            // update title
            if (request.Title != null && planner.Title != request.Title)
            {
                planner.Title = request.Title;
            }
            // update Purpose
            if (request.Purpose != null && planner.Purpose != request.Purpose)
            {
                planner.Purpose = request.Purpose;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
