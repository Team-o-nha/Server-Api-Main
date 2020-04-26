using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;

namespace ColabSpace.Application.TaskItems.Models
{
    public class RelatedObjectModel : IMapFrom<RelatedObject>
    {
        public string ObjectId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
