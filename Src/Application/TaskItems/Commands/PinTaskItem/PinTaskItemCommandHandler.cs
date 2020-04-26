using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Commands.PinTaskItem
{
    public class PinTaskItemCommandHandler : IRequestHandler<PinTaskItemCommand, TaskItemModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public PinTaskItemCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskItemModel> Handle(PinTaskItemCommand request, CancellationToken cancellationToken)
        {
            // lay thong tin task item
            var taskItem = _context.TaskItems.Find(request.Id);

            if (taskItem == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.Id);
            }

            // cap nhat lai task item
            taskItem.IsPin = request.IsPin;
            taskItem.PinnedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TaskItemModel>(taskItem);
        }
    }
}
