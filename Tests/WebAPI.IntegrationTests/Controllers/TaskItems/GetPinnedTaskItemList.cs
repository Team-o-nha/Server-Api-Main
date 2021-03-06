﻿using ColabSpace.Application.TaskItems.Models;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.TaskItems
{
    public class GetPinnedTaskItemList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetPinnedTaskItemList(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidTeamId_ValidKeyword_ReturnsTaskItemsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var validId = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae");

            var response = await client.GetAsync($"/api/TaskItems/GetPin/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TaskItemModel>>(response);

            vm.ShouldBeOfType<List<TaskItemModel>>();
            vm.Count.ShouldBe(2);
        }
    }
}
