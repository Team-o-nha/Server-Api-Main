using ColabSpace.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class Planner : AuditableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string Purpose { get; set; }

        public ICollection<Milestone> Milestones { get; set; }

        public Guid? TeamId { get; set; }

    }
}
