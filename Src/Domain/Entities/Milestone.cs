using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class Milestone
    {
        public string Title { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public ICollection<Guid> TaskIds { get; set; }
    }
}
