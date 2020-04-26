using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;

namespace ColabSpace.Application.Common.Models
{
    public class TagModel : IMapFrom<Tag>
    {
        public string TagName { get; set; }
    }
}
