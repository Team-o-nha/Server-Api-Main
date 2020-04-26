using ColabSpace.Application.Conversations.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Conversations.Commands.HideConversation
{
    public class HideConversationCommand : IRequest
    {
        public string ConversationId;
    }
}
