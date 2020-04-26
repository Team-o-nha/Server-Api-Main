using ColabSpace.Application.Common.Models;

namespace ColabSpace.WebAPI.Services
{
    public static class CopyHandler
    {
        public static UserDto UserProperty(Microsoft.Graph.User graphUser)
        {
            UserDto user = new UserDto
            {
                UserId = graphUser.Id,
                GivenName = graphUser.GivenName,
                Surname = graphUser.Surname,
                UserPrincipalName = graphUser.UserPrincipalName,
                Email = graphUser.Mail,
                DisplayName = graphUser.DisplayName
            };

            return user;
        }

        public static Group GroupProperty(Microsoft.Graph.Group graphGroup)
        {
            Group group = new Group
            {
                id = graphGroup.Id,
                displayName = graphGroup.DisplayName
            };

            return group;
        }
    }
}
