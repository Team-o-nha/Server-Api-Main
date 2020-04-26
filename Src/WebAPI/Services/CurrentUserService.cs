using ColabSpace.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ColabSpace.WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = ((ClaimsIdentity)httpContextAccessor.HttpContext?.User?.Identity)?.FindFirst("Name").Value;
        }

        public string UserId { get; }

        public string UserName { get; }
    }
}
