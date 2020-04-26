using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskItemCommandHandler(IColabSpaceDbContext context, IMediator mediator
            , ICurrentUserService currentUserService)
        {
            _context = context;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems.FindAsync(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(TaskItem), request.Id);
            }

            var team = await _context.Teams.FindAsync(request.TeamId);
            if (team == null)
            {
                throw new NotFoundException(nameof(Team), request.TeamId);
            }
            // check login user own task
            if (_currentUserService.UserId != task.CreatedBy.UserOid
                && _currentUserService.UserId != task.Assignee.UserOid)
            {
                var loginUser = team.Users.First(u => u.UserOid == _currentUserService.UserId);
                if (loginUser.TeamRole != "Leader")
                {
                    throw new NotOwnedException(nameof(TaskItem), task.Id);
                }
            }
            List<AttachFile> attachFiles = new List<AttachFile>();
            if (request.AttachFiles != null)
            {
                foreach (AttachFileModel file in request.AttachFiles)
                {
                    attachFiles.Add(new AttachFile()
                    {
                        FileName = file.FileName,
                        FileStorageName = file.FileStorageName,
                        BlobStorageUrl = file.BlobStorageUrl,
                        FileSize = file.FileSize,
                        ThumbnailImage = file.ThumbnailImage
                    });
                }
            }
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

            // related Object
            List<RelatedObject> listRelatedObject = new List<RelatedObject>();
            if (request.Relations != null)
            {
                foreach (RelatedObjectModel relatedObject in request.Relations)
                {
                    listRelatedObject.Add(new RelatedObject()
                    {
                        ObjectId = relatedObject.ObjectId,
                        Title = relatedObject.Title,
                        Type = relatedObject.Type,
                        Url = relatedObject.Url
                    });
                }
            }

            var history = createHistory(request, task);

            task.Name = request.Name;
            task.Status = request.Status;
            task.TeamId = request.TeamId;
            task.Description = request.Description;
            task.Assignee = new User { UserOid = request.Assignee?.UserId.ToString(), DisplayName = request.Assignee?.DisplayName };
            task.AttachFiles = attachFiles;
            task.CreatedBy = new User { UserOid = request.CreatedBy?.UserId.ToString(), DisplayName = request.CreatedBy?.DisplayName };
            task.Tags = tags;
            task.Deadline = request.Deadline?.ToUniversalTime();
            task.Relations = listRelatedObject;

            if (!String.IsNullOrEmpty(history?.Title))
            {
                if (task.Histories == null || task.Histories?.Count == 0)
                {
                    List<History> histories = new List<History>();
                    if (history == null) { }
                    else
                    {
                        histories.Add(history);
                        task.Histories = histories;
                    }
                }
                else
                {
                    if (history == null) { }
                    else
                    {
                        task.Histories.Add(history);

                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        private History createHistory(UpdateTaskItemCommand request, TaskItem task)
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
            if (task.Name != request.Name)
            {
                history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Name: \"{0}\" -> \"{1}\"", task.Name, request.Name);
            }
            if (task.Description != request.Description)
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Description: \"{0}\" -> \"{1}\"", task.Description, request.Description);
                }
                else
                {
                    if (task.Description != null)
                    {
                        history.Title += String.Format(", Description: \"{0}\" -> \"{1}\"", task.Description, request.Description);
                    }
                    else
                    {
                        history.Title += String.Format(", Description: \"\" -> \"{0}\"", request.Description);
                    }
                }
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
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Status: \"{0}\" -> \"{1}\"", statusName1, statusName2);
                }
                else
                {
                    history.Title += String.Format(", Status: \"{0}\" -> \"{1}\"", statusName1, statusName2);
                }
            }

            if (task.Deadline != request.Deadline)
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: " + String.Format("Deadline: \"{0}\" -> \"{1}\"", task.Deadline, request.Deadline);
                }
                else
                {
                    if (task.Deadline != null)
                    {
                        history.Title += String.Format(", Deadline: \"{0}\" -> \"{1}\"", task.Deadline, request.Deadline);
                    }
                    else
                    {
                        history.Title += String.Format(", Deadline: \"\" -> \"{0}\"", request.Deadline);
                    }
                }
            }

            // tags number change
            if (request.Tags == null || request.Tags?.Count == 0)
            {
                // not update
            }
            else if (task.Tags?.Count != request.Tags?.Count)
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: Tags";
                }
                else
                {
                    history.Title += String.Format(", Tags");
                }
            }
            else
            {
                // tags change
                int count = 0;
                foreach (var tag in request.Tags)
                {
                    count = task.Tags.Where(x => x.TagName == tag.TagName).Count();
                }
                if (count == 0)
                {
                    if (String.IsNullOrEmpty(history.Title))
                    {
                        history.Title = _currentUserService.UserName + " has updated task: Tags";
                    }
                    else
                    {
                        history.Title += String.Format(", Tags");
                    }
                }
            }

            // attach files number change
            if (task.AttachFiles == null && request.AttachFiles?.Count == 0)
            {
                // not update
            }
            else if (task.AttachFiles?.Count != request.AttachFiles?.Count)
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: AttachFiles";
                }
                else
                {
                    history.Title += String.Format(", AttachFiles");
                }
            }
            else
            {
                // attach files change
                int count = 0;
                foreach (var file in request.AttachFiles)
                {
                    count = task.AttachFiles.Where(x => x.BlobStorageUrl == file.BlobStorageUrl).Count();
                }
                if (count == 0)
                {
                    if (String.IsNullOrEmpty(history.Title))
                    {
                        history.Title = _currentUserService.UserName + " has updated task: AttachFiles";
                    }
                    else
                    {
                        history.Title += String.Format(", AttachFiles");
                    }
                }
            }

            // relations
            if ((task.Relations == null && request.Relations?.Count == 0) || (task.Relations == null && request.Relations == null))
            {
                // not update
            }
            else if (task.Relations?.Count != request.Relations?.Count)
            {
                if (String.IsNullOrEmpty(history.Title))
                {
                    history.Title = _currentUserService.UserName + " has updated task: Relations";
                }
                else
                {
                    history.Title += String.Format(", Relations");
                }
            }
            else
            {
                // Relations changes
                int count = 0;
                foreach (var obj in request.Relations)
                {
                    count = task.Relations.Where(x => x.ObjectId == obj.ObjectId).Count();
                }
                if (count == 0)
                {
                    if (String.IsNullOrEmpty(history.Title))
                    {
                        history.Title = _currentUserService.UserName + " has updated task: Relations";
                    }
                    else
                    {
                        history.Title += String.Format(", Relations");
                    }
                }
            }

            if (history.Title == null)
            {
                history = null;
            }

            return history;
        }
    }
}
