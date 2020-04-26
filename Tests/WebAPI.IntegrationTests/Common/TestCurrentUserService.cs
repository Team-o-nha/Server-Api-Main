using ColabSpace.Application.Common.Interfaces;

namespace ColabSpace.WebAPI.IntegrationTests.Common
{
    public class TestCurrentUserService : ICurrentUserService
    {
        public string UserId => "00000000-0000-0000-0000-000000000000";

        public string UserName => "test@gmail.com";

        public bool Authenticated => true;
    }
}
