using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandHandler : IRequestHandler<DeletePlannerCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public DeletePlannerCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(DeletePlannerCommand request, CancellationToken cancellationToken)
        {
            var planner = await _context.Planners.FindAsync(request.Id);
            if (planner == null)
            {
                throw new NotFoundException(nameof(Planner), request.Id);
            }
            _context.Planners.Remove(planner);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
