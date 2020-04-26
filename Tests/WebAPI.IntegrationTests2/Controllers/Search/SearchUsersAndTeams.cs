using ColabSpace.Application.Common.Models;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Search
{
    public class SearchUsersAndTeams : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SearchUsersAndTeams(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidTeamId_ReturnsTaskItemsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = "Team";

            var response = await client.GetAsync($"/api/Search/userAndTeam/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            vm.ShouldBeOfType<List<SearchModel>>();
            vm.Count.ShouldBe(5);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInValidTeamId_ReturnsEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = "invalid";

            var response = await client.GetAsync($"/api/Search/userAndTeam/?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            vm.Count.ShouldBe(0);
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidTeamIdNoKeyword_ReturnsListUserInTeam()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validTeamId = "de14a885-71d4-4da0-bb17-048d74d33adc"; //2 members in team
            var keyword = String.Empty;

            var response = await client.GetAsync($"/api/Search/userInTeam/{validTeamId}?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<UserModel> vm = await IntegrationTestHelper.GetResponseContent<List<UserModel>>(response);

            vm.ShouldBeOfType<List<UserModel>>();
            vm.Count.ShouldBe(2);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenInvalidTeamId_ReturnsNotFoundException()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var inValidTeamId = "de14a885-71d4-4da0-bb17-048d74d33add"; 

            var response = await client.GetAsync($"/api/Search/userInTeam/{inValidTeamId}");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidTeamIdAndValidKeyword_ReturnsUserHaveNameContainKeyword()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var validTeamId = "de14a885-71d4-4da0-bb17-048d74d33adc"; //2 members in team
            var keyword = "User123";

            var response = await client.GetAsync($"/api/Search/userInTeam/{validTeamId}?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<UserModel> vm = await IntegrationTestHelper.GetResponseContent<List<UserModel>>(response);

            vm.ShouldBeOfType<List<UserModel>>();
            vm.Count.ShouldBe(1);
            vm[0].DisplayName.ShouldContain(keyword);

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenEmptyKeyword_ShouldReturnEmptyList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = String.Empty;

            var response = await client.GetAsync($"/api/Search/users-groups?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<UserModel> vm = await IntegrationTestHelper.GetResponseContent<List<UserModel>>(response);

            vm.ShouldBeOfType<List<UserModel>>();
            vm.ShouldBeEmpty();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidKeyword_ShouldReturnUsersList()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = "User";

            var response = await client.GetAsync($"/api/Search/users-groups?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<SearchModel> vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            vm.ShouldBeOfType<List<SearchModel>>();
            vm.ShouldNotBeEmpty();
            foreach (var m in vm)
            {
                m.Name.ShouldContain(keyword);
            }

            // release DB
            _factory.DisposeDbForTests(context);
        }

        //search users
        [Fact]
        public async Task GivenValidKeyword_ShouldReturnUsersList2()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = "User";

            var response = await client.GetAsync($"/api/Search/users?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<SearchModel> vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            vm.ShouldBeOfType<List<SearchModel>>();
            vm.ShouldNotBeEmpty();
            foreach (var m in vm)
            {
                m.Name.ShouldContain(keyword);
            }

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenEmptyKeyword_ShouldReturnEmptyList2()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = String.Empty;

            var response = await client.GetAsync($"/api/Search/users?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<SearchModel> vm = await IntegrationTestHelper.GetResponseContent<List<SearchModel>>(response);

            vm.ShouldBeOfType<List<SearchModel>>();
            vm.ShouldBeEmpty();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        //SearchMessages
        [Fact]
        public async Task GivenEmptyKeyword_ShouldReturnListResult()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = String.Empty;

            var response = await client.GetAsync($"/api/Search/messages?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<MessageChatModel> vm = await IntegrationTestHelper.GetResponseContent<List<MessageChatModel>>(response);

            vm.ShouldBeOfType<List<MessageChatModel>>();

            vm.ShouldNotBeNull();
            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidKeyword_ShouldListMessageContainKeyword()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();
            var keyword = "a";

            var response = await client.GetAsync($"/api/Search/messages?keyword={keyword}");

            response.EnsureSuccessStatusCode();

            List<MessageChatModel> vm = await IntegrationTestHelper.GetResponseContent<List<MessageChatModel>>(response);

            vm.ShouldBeOfType<List<MessageChatModel>>();
            vm.Count.ShouldBeGreaterThan(1);

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
