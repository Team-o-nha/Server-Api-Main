﻿using System.Threading.Tasks;
using Xunit;
using Shouldly;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using ColabSpace.Application.Teams.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Teams
{
    public class GetAllByTeamName : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GetAllByTeamName(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsTeamsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            string keword = "Team 1";

            var response = await client.GetAsync($"/api/Team/GetAllByTeamName/{keword}");
            Debug.WriteLine("33333333");
            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<TeamModel>>(response);

            vm.ShouldBeOfType<List<TeamModel>>();
            vm.Count.ShouldBe(1);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
