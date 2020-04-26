using ColabSpace.Application.TaskItems.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class SearchTaskInTeam : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SearchTaskInTeam(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var invalidId = Guid.NewGuid();

            //var keyword = "task";
            var keyword = "";

            var response = await client.GetAsync($"/api/TaskItems/GetAll/{invalidId}/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TaskItemModel>>(response);

            vm.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GivenValidTeamId_InvalidKeyword_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            //var keyword = "invalid";
            var keyword = "";

            var response = await client.GetAsync($"/api/TaskItems/GetAll/{validId}/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TaskItemModel>>(response);

            //vm.Count.ShouldBe(0);
            vm.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GivenValidTeamId_ValidKeyword_ReturnsTaskItemsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            //var keyword = "1";
            var keyword = "";

            var response = await client.GetAsync($"/api/TaskItems/GetAll/{validId}/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TaskItemModel>>(response);

            vm.ShouldBeOfType<List<TaskItemModel>>();
            vm.Count.ShouldBe(2);
            vm.ForEach(t => (t.Name.Contains(keyword) || t.CreatedBy.DisplayName.Contains(keyword)).ShouldBeTrue());
        }
    }
}
