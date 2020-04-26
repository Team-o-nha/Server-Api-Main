using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Notifications.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ColabSpace.Application.Notifications.Queries.GetNotificationsList
{
    public class GetNotificationsByUserIdQueryHandler : IRequestHandler<GetNotificationsByUserIdQuery, NotificationsPagingModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly int pageSize = 20;

        public GetNotificationsByUserIdQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NotificationsPagingModel> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var notifications = new List<Notification>();

            // kiem tra neu khong truyen PageIndex thi lay trang dau
            request.PageIndex = request.PageIndex ?? 1;

            var feedQuery = _context.Notifications
                // sap xep theo moi nhat
                .OrderByDescending(x => x.Created)
                // chi lay 1 page, khong load het tat ca cac feed
                .Skip(((int)request.PageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsQueryable();

            var unreadFeedCount = 0;

            // lay feed o man hinh [Home Page] (lay tat ca feed cua cac team)
            if (string.IsNullOrEmpty(request.TeamId))
            {
                notifications = await feedQuery
                    .Where(p => p.ToUser.UserOid == request.UserId.ToString())
                    .ToListAsync(cancellationToken);

                unreadFeedCount = _context.Notifications
                    .Where(notification => notification.isRead == false
                        && notification.ToUser.UserOid == request.UserId.ToString())
                    .Select(notification => notification.Id)
                    .AsEnumerable()
                    .Count();
            }
            else
            {
                // lay feed cua team o man hinh [Team Page] (chi lay feed cua team)
                notifications = await feedQuery
                    .Where(p => p.ToUser.UserOid == request.UserId.ToString() && p.TeamId == request.TeamId)
                    .ToListAsync(cancellationToken);

                unreadFeedCount = _context.Notifications
                    .Where(notification => notification.isRead == false
                        && notification.TeamId == request.TeamId
                        && notification.ToUser.UserOid == request.UserId.ToString())
                    .Select(notification => notification.Id)
                    .AsEnumerable()
                    .Count();
            }

            var notificationsPagingModel = new NotificationsPagingModel
            {
                UnreadFeedCount = unreadFeedCount,
                Notifications = _mapper.Map<List<Notification>, List<NotificationModel>>(notifications)
            };

            return notificationsPagingModel;
        }
    }
}


