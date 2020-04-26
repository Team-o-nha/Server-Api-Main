using ColabSpace.Application.MessageChats.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Queries.GetMessageChatById
{
    public class GetMessageChatByIdQuery : IRequest<MessageChatModel>
    {
        public Guid MessageChatId { get; set; }
    }
}
