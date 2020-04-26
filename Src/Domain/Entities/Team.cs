using ColabSpace.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class Team : AuditableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
