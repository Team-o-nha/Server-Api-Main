using ColabSpace.Application.Notifications.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Notifications
{
    public class GetNotificationsByToUserId : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetNotificationsByToUserId(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUserId_ReturnNotificationsVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validUserId = new Guid("66EDB7C7-11BF-40A5-94AB-75A3364FEF60");

            var response = await client.GetAsync($"/api/Notifications/{validUserId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<NotificationsPagingModel>(response);

            vm.ShouldBeOfType<NotificationsPagingModel>();
            vm.Notifications.Count.ShouldBe(1);
        }
    }
}
