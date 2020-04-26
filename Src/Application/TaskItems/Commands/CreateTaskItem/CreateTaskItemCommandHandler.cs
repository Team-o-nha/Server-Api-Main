using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, Guid>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateTaskItemCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams.FindAsync(request.TeamId);
            if (team == null)
            {
                throw new NotFoundException(nameof(Team), request.TeamId);
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

            var taskItem = new TaskItem()
            {
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                Assignee = new User { UserOid = request.Assignee.UserId.ToString(), DisplayName = request.Assignee.DisplayName },
                AttachFiles = attachFiles,
                CreatedBy = new User { UserOid = request.CreatedBy.UserId.ToString(), DisplayName = request.CreatedBy.DisplayName },
                TeamId = request.TeamId,
                Deadline = request.Deadline?.ToUniversalTime(),
                Tags = tags,
                RelatedMessagesId = request.RelatedMessagesId,
                Relations = listRelatedObject
            };

            // history update 
            var historiesList = new List<History>();

            historiesList.Add(new History()
            {
                Title = _currentUserService.UserName + " created a new task",
                Type = "Task",
                Content = "Create",
                UserId = _currentUserService.UserId,
                UserName = _currentUserService.UserName,
                Date = DateTime.UtcNow
            });

            taskItem.Histories = historiesList;

            _context.TaskItems.Add(taskItem);

            // login user la assignee thi khong tao feed
            if (_currentUserService.UserId != taskItem.Assignee.UserOid)
            {
                // tao feed khi tao moi 1 task
                var notification = new Notification
                {
                    isRead = false,
                    TaskItemId = taskItem.Id.ToString(),
                    TeamId = taskItem.TeamId.ToString(),
                    ToUser = taskItem.Assignee,
                    Type = "AddTask",
                    RegUserName = taskItem.CreatedBy.DisplayName,
                    RegUserId = taskItem.CreatedBy.UserOid,
                    Title = taskItem.CreatedBy.DisplayName + " created task " + taskItem.Name
                };

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return taskItem.Id;
        }
    }
}
