using ColabSpace.Application.TaskItems.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.MessageChats.Commands.CreateMessageChat
{
    public class CreateMessageChatCommand : IRequest<Guid>
    {
        public string Content { get; set; }

        public Guid ConversationId { get; set; }

        public ICollection<AttachFileModel> AttachFileList { get; set; }

        public string RegUserId { get; set; }

        public string RegUserName { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public Guid? RelatedTaskId { get; set; }
    }
}
