using ColabSpace.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class Conversation : AuditableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string ChannelDescription { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public ICollection<User> Members { get; set; }

        public string TeamId { get; set; }

        public bool isPublic { get; set; }
    }
}
