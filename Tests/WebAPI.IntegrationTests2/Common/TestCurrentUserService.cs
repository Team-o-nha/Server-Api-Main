using ColabSpace.Application.Common.Interfaces;

namespace ColabSpace.WebAPI.IntegrationTests2.Common
{
    public class TestCurrentUserService : ICurrentUserService
    {
        public string UserId => "020cdee0-8ecd-408a-b662-cd4d9cdf0100";
        public string UserName => "testuser";
        public bool Authenticated => true;
    }
}
