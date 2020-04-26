using ColabSpace.WebAPI.IntegrationTests.Common;
using ColabSpace.WebAPI.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.MessageChats
{
    public class GetMessageById : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetMessageById(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GivenValidRelatedMessageId_ReturnsMessage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("B73477A4-F61D-46FA-873C-7D71C01DFBDF");

            var response = await client.GetAsync($"/api/MessageChat/GetMessage/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<MessageDto>(response);

            vm.ShouldBeOfType<MessageDto>();
            vm.MessageId.ShouldBe(validId.ToString());
            vm.Message.ShouldBe("abc");
            vm.Name.ShouldBe("Long");
        }

        [Fact]
        public async Task GivenInValidRelatedMessageId_ReturnsNull()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/MessageChat/GetMessage/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
