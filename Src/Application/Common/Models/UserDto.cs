using System.Collections.Generic;

namespace ColabSpace.Application.Common.Models
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
        public string Email { get; set; }
    }

    public class UsersDto
    {
        public int ItemsPerPage { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }
        public List<UserDto> Resources { get; set; }
    }
}
