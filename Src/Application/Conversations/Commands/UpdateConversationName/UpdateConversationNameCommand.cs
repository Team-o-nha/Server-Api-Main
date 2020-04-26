using ColabSpace.Application.Teams.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Commands.UpdateConversationName
{
    public class UpdateConversationNameCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public ICollection<UserModel> Members { get; set; }
    }
}
