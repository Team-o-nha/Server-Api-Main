using ColabSpace.Application.Notifications.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Notifications
{
    public class GetNotificationsByTeam : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetNotificationsByTeam(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUserId_ReturnReturnNotificationInTeam()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validUserId = new Guid("66edb7c7-11bf-40a5-94ab-75a3364fef60");

            var response = await client.GetAsync($"/api/Notifications/ByTeam/{validUserId}");

            response.EnsureSuccessStatusCode();

            List<NotificationModel> vm = await IntegrationTestHelper.GetResponseContent<List<NotificationModel>>(response);

            vm.Count.ShouldBe(1);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
