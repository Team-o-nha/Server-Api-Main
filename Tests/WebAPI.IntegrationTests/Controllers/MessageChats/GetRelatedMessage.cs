using ColabSpace.WebAPI.IntegrationTests.Common;
using ColabSpace.WebAPI.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.MessageChats
{
    public class GetRelatedMessage : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetRelatedMessage(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidRelatedMessageId_ReturnsTaskItemsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            var validTaskId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/MessageChat/GetRelatedMessage/{validId}?relatedTaskId={validTaskId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<MessageDto>>(response);

            vm.ShouldBeOfType<List<MessageDto>>();
            vm.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GivenInValidRelatedMessageId_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/MessageChat/GetRelatedMessage/{invalidId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<MessageDto>>(response);

            vm.Count.ShouldBe(0);
        }
    }
}
