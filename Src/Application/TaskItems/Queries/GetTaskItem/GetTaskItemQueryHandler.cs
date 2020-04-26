using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Queries.GetTaskItem
{
    public class GetTaskItemQueryHandler : IRequestHandler<GetTaskItemQuery, TaskItemModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetTaskItemQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TaskItemModel> Handle(GetTaskItemQuery request, CancellationToken cancellationToken)
        {
            var taskItem = await _context.TaskItems.FindAsync(request.TaskItemId);

            if (taskItem == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.TaskItemId);
            }

            return _mapper.Map<TaskItem, TaskItemModel>(taskItem);
        }
    }
}
