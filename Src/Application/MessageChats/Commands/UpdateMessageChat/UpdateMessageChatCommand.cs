using MediatR;
using System;

namespace ColabSpace.Application.MessageChats.Commands.UpdateMessageChat
{
    public class UpdateMessageChatCommand : IRequest<string>
    {
        public Guid MessageId { get; set; }

        public string ReactionType { get; set; }

        public bool? IsPin { get; set; }
    }
}
