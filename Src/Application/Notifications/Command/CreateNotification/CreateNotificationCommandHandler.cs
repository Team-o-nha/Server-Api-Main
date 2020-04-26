using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Notifications.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Notifications.Command.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public CreateNotificationCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
             
            if (request.ToUser == null)
            {
                request.ToUser = new List<UserModel>();
            }
            var validNotificationType = new List<string> { "Mention", "AddGroup", "Reaction", "..." };
            if (!validNotificationType.Contains(request.Type))
            {
                throw new NotTypeException(request.Type, validNotificationType);
            }

            var notificationList = new List<Notification>() { };

            if (request.Type == "Mention")
            {
                if (string.IsNullOrEmpty(request.TeamId))
                {
                    foreach (var user in request.ToUser)
                    {
                        var notification = getNewNotification(request);
                        notification.Title = request.RegUserName + " mentioned you in a message";
                        notification.ToUser = new User() { DisplayName = user.DisplayName, UserOid = user.UserId.ToString() };
                        notification.MessageContent = request.MessageContent;
                        notification.MessageId = request.MessageId;
                        notificationList.Add(notification);
                    }
                }
                else
                {
                    foreach (var user in request.ToUser)
                    {
                        var notification = getNewNotification(request);
                        notification.Title = request.RegUserName + " mentioned you in a channel";
                        notification.ToUser = new User() { DisplayName = user.DisplayName, UserOid = user.UserId.ToString() };
                        notification.MessageContent = request.MessageContent;
                        notification.MessageId = request.MessageId;
                        notificationList.Add(notification);
                    }
                }
            }
            else if (request.Type == "AddGroup")
            {
                foreach (var user in request.ToUser)
                {
                    var notification = getNewNotification(request);
                    notification.Title = request.RegUserName + " added you into a group";
                    notification.MessageContent = "";
                    notification.MessageId = "";
                    notification.ToUser = new User() { DisplayName = user.DisplayName, UserOid = user.UserId.ToString() };
                    notificationList.Add(notification);
                }
            }
            else if (request.Type == "Reaction")
            {
                foreach (var user in request.ToUser)
                {
                    var notification = getNewNotification(request);
                    notification.Title = request.RegUserName + " added a reaction";
                    notification.MessageContent = request.MessageContent;
                    notification.MessageId = request.MessageId;
                    notification.ToUser = new User() { DisplayName = user.DisplayName, UserOid = user.UserId.ToString() };
                    notificationList.Add(notification);
                }
            }
            _context.Notifications.AddRange(notificationList);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public Notification getNewNotification(CreateNotificationCommand request)
        {
            return new Notification
            {
                isRead = false,
                URL = "",
                Type = request.Type,
                ConversationId = request.ConversationId,
                Created = DateTime.UtcNow,
                RegUserId = request.RegUserId,
                CreatedBy = request.RegUserId,
                RegUserName = request.RegUserName,
                TeamId = request.TeamId,
                TeamName = request.TeamName
            };
        }
    }
}

