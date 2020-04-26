using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;

namespace ColabSpace.Application.MessageChats.Models
{
    public class ReactionModel : IMapFrom<Reaction>
    {
        public string ReactorId { get; set; }

        public string ReactorName { get; set; }

        public string ReactionType { get; set; }
    }
}
