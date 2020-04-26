using ColabSpace.Application.Notifications.Command;
using ColabSpace.Application.Notifications.Command.CreateNotification;
using ColabSpace.Application.Notifications.Models;
using ColabSpace.Application.Notifications.Queries.GetNotificationsByTeam;
using ColabSpace.Application.Notifications.Queries.GetNotificationsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        [HttpGet("{userId}")]
        public async Task<NotificationsPagingModel> GetNotificationsByUserId(Guid userId, [FromQuery] string teamId, [FromQuery] int? pageIndex)
        {
            return await Mediator.Send(new GetNotificationsByUserIdQuery
            {
                UserId = userId,
                TeamId = teamId,
                PageIndex = pageIndex
            });
        }

        /**
         * Dung cho man hinh danh sach team va cho Feed o side menu khi o trong [Team Page]
         * Load cac Notification theo tung Team
         */
        [HttpGet("ByTeam/{userId}")]
        public async Task<IEnumerable<NotificationByTeamModel>> GetNotificationsByTeam(Guid userId)
        {
            return await Mediator.Send(new GetNotificationsByTeamQuery { UserId = userId });
        }

        [HttpPost("Read/{notificationId}")]
        public async Task<ActionResult> Read(string notificationId)
        {
            await Mediator.Send(new ReadNotificationCommand { NotificationId = new Guid(notificationId) });

            return NoContent();
        }

        [HttpPost("UnRead/{notificationId}")]
        public async Task<ActionResult> UnRead(string notificationId)
        {
            await Mediator.Send(new UnReadNotificationCommand { NotificationId = new Guid(notificationId) });

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateNotificationCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

    }
}