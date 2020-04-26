using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, TaskItemModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTaskItemCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService
            , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /**
         * メンバーがタスク削除/Member Delete Task
         * リーダーがすべてメンバーのタスク削除/Leader Delete Task of All Members
         */
        public async Task<TaskItemModel> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new ArgumentNullException("Task item id can not be null!");
            }

            var task = await _context.TaskItems.FindAsync(Guid.Parse(request.Id));

            if (task == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.Id);
            }

            // check login user own task
            if (_currentUserService.UserId != task.CreatedBy.UserOid && _currentUserService.UserId != task.Assignee.UserOid)
            {
                var team = await _context.Teams.FindAsync(task.TeamId);
                if (team == null)
                {
                    throw new NotFoundException(nameof(team), task.TeamId);
                }
                else
                {
                    // search leader in team
                    var loginUser = team.Users.First(u => u.UserOid == _currentUserService.UserId);
                    if (loginUser.TeamRole != "Leader")
                    {
                        throw new NotOwnedException(nameof(TaskItem), task.Id);
                    }
                }
            }

            _context.TaskItems.Remove(task);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TaskItem, TaskItemModel>(task);
        }
    }
}
