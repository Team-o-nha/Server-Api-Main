using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Conversations.Models
{
    public class ConversationModel : IMapFrom<Conversation>
    {
        public Guid Id { get; set; }

        public string ChannelDescription { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public ICollection<UserModel> Members { get; set; }

        public string TeamId { get; set; }

        public bool IsPublic { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
