using System;

namespace ColabSpace.Domain.Entities
{
    public class User
    {
        public string UserOid { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public string TeamRole { get; set; }
        public bool isHidden { get; set; }
        public DateTime LastSeenTime { get; set; }
    }
}
