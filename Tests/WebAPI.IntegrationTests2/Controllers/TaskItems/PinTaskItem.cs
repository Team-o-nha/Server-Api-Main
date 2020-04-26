using ColabSpace.Application.TaskItems.Commands.PinTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.TaskItems
{
    public class PinTaskItem : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PinTaskItem(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ReturnOkAndUpdatedTaskItem()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var taskId = new Guid("de14a885-71d4-4da0-bb17-048d74d33ada");
            PinTaskItemCommand command = new PinTaskItemCommand()
            {
                Id = taskId,
                IsPin = true
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/Pin/{taskId}", content);

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<TaskItemModel>(response);

            vm.ShouldBeOfType<TaskItemModel>();
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidRequest_ReturnOkAndUpdatedTaskItem()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var taskId = new Guid("de14a885-71d4-4da0-bb17-048d74d33adb");
            PinTaskItemCommand command = new PinTaskItemCommand()
            {
                Id = Guid.Parse("de14a885-71d4-4da0-bb17-048d74d33dad"),
                IsPin = true
            };

           var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"/api/TaskItems/Pin/{taskId}", content);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
