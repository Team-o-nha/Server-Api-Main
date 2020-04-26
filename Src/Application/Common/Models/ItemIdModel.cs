using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;
using System;

namespace ColabSpace.Application.Common.Models
{
    public class ItemIdModel: IMapFrom<ItemId>
    {
        public Guid? Id { get; set; }
    }
}
