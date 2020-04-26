using ColabSpace.Application.Notifications.Models;
using ColabSpace.Application.Teams.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Notifications.Command.CreateNotification
{
    public class CreateNotificationCommand : IRequest
    {
        public string Type { get; set; }
        //public string Title { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string ConversationId { get; set; }
        public string ConversationName { get; set; }
        public string MessageId { get; set; }
        public string MessageContent { get; set; }
        public string RegUserName { get; set; }
        public string RegUserId { get; set; }
        public ICollection<UserModel> ToUser { get; set; }
        public bool isRead { get; set; }
        public string URL { get; set; }
    }
}
