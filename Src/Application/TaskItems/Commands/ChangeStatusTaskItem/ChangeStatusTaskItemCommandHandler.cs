using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Commands.ChangeStatusTaskItem
{
    public class ChangeStatusTaskItemCommandHandler : IRequestHandler<ChangeStatusTaskItemCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusTaskItemCommandHandler(IColabSpaceDbContext context, IMediator mediator
            , ICurrentUserService currentUserService)
        {
            _context = context;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangeStatusTaskItemCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems.FindAsync(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.Id);
            }

            var team = await _context.Teams.FindAsync(task.TeamId);
            // check login user own task
            if (_currentUserService.UserId != task.CreatedBy.UserOid
                && _currentUserService.UserId != task.Assignee.UserOid
                && team != null)
            {
                var loginUser = team.Users.First(u => u.UserOid == _currentUserService.UserId);
                if (loginUser.TeamRole != "Leader")
                {
                    throw new NotOwnedException(nameof(TaskItem), task.Id);
                }
            }

            if (task.Histories == null || task.Histories?.Count == 0)
            {
                List<History> histories = new List<History>();
                histories.Add(createHistory(request, task));
                task.Histories = histories;
            }
            else
            {
                task.Histories.Add(createHistory(request, task));
            }

            // team leader
            var teamLeader = GetTeamLeader(team);
            task.Status = request.Status;
            task.Assignee = new User { UserOid = request.Assignee.UserId.ToString(), DisplayName = request.Assignee.DisplayName };

            // tao notification neu hoan thanh task
            if (task.Status == 3 && teamLeader.UserOid != _currentUserService.UserId)
            {
                var notification = new Notification
                {
                    isRead = false,
                    TaskItemId = task.Id.ToString(),
                    TeamId = team.Id.ToString(),
                    ToUser = teamLeader,
                    Type = "DoneTask",
                    RegUserName = _currentUserService.UserName,
                    RegUserId = _currentUserService.UserId,
                    Title = task.Assignee.DisplayName + " finished task" + task.Name
                };

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private History createHistory(ChangeStatusTaskItemCommand request, TaskItem task)
        {

            // history update 
            var history = new History()
            {
                Type = "Task",
                Content = "Update",
                UserId = _currentUserService.UserId,
                UserName = _currentUserService.UserName,
                Date = DateTime.UtcNow
            };

            if (task.Status != request.Status)
            {
                string statusName1 = "";
                string statusName2 = "";

                statusName1 = task.Status switch
                {
                    1 => "To Do",
                    2 => "In Progress",
                    3 => "Done",
                    _ => "Unknown"
                };
                statusName2 = request.Status switch
                {
                    1 => "To Do",
                    2 => "In Progress",
                    3 => "Done",
                    _ => "Unknown"
                };
                history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Status: \"{0}\" -> \"{1}\"", statusName1, statusName2);
            }

            if (task.Assignee?.UserOid != request.Assignee.UserId.ToString())
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Assignee: \"{0}\" -> \"{1}\"", task.Assignee.DisplayName, request.Assignee.DisplayName);
                }
                else
                {
                    if (task.Assignee == null)
                    {
                        history.Title += String.Format(", Assignee: \"\" -> \"{0}\"", request.Assignee.DisplayName);
                    }
                    else
                    {
                        history.Title += String.Format(", Assignee: \"{0}\" -> \"{1}\"", task.Assignee.DisplayName, request.Assignee.DisplayName);
                    }
                }
            }
            return history;
        }

        private User GetTeamLeader(Team team)
        {
            foreach (var member in team.Users)
            {
                if (member.TeamRole == "Leader")
                {
                    return member;
                }
            }

            return null;
        }
    }
}
