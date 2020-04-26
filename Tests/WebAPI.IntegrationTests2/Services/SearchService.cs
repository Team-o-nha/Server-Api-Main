using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Commands.UpdateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using ColabSpace.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Services
{
    public class SearchService : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public SearchService(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        protected TService GetService<TService>() where TService : class
        {
            return _factory.GetService<TService>();
        }

        [Fact]
        public async void GivenValidUserIdWithValidKeyword_ShouldReturnSearchData()
        {
            var service = GetService<ISearchService>();
            Assert.NotNull(service);

            var result = service.SearchMessages("020cdee0-8ecd-408a-b662-cd4d9cdf0100", "Team");
            Assert.NotNull(result);
        }

        [Fact]
        public async void GivenInvalidUserIdWithValidKeyword_ShouldEmptyList()
        {
            var service = GetService<ISearchService>();
            Assert.NotNull(service);

            var result = service.SearchMessages(Guid.NewGuid().ToString(), "Team");
            Assert.Empty(result);
        }
    }
}
