using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Directory
{
    public class Directory : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public Directory(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUserId_ShouldReturnUserData()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var validUserId = Guid.Parse("020cdee0-8ecd-408a-b662-cd4d9cdf0100"); // login user id

            var response = await client.GetAsync($"/api/Directory/users/{validUserId}");

            UserDto content = await IntegrationTestHelper.GetResponseContent<UserDto>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            content.ShouldBeOfType(typeof(UserDto));
            content.UserId.ShouldBe(validUserId.ToString());
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidUserId_ShouldReturnNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var invalidUserId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Directory/users/{invalidUserId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task ReturnAllUsers()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var response = await client.GetAsync($"/api/Directory/users/");

            UsersDto content = await IntegrationTestHelper.GetResponseContent<UsersDto>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            content.Resources.Count.ShouldBeGreaterThan(0);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidTeamId_ShouldReturnUserNotInTeam()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var validTeamId = Guid.Parse("7567347E-0580-4807-97F1-8EDAD42C9758");

            var response = await client.GetAsync($"/api/Directory/users/notInTeam/{validTeamId}");

            UsersDto content = await IntegrationTestHelper.GetResponseContent<UsersDto>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            content.Resources.Count.ShouldBeGreaterThan(0);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidTeamId_ShouldReturnNotFoundException()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var validTeamId = Guid.NewGuid();

            var response = await client.GetAsync($"/api/Directory/users/notInTeam/{validTeamId}");

            UsersDto content = await IntegrationTestHelper.GetResponseContent<UsersDto>(response);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
