using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Planners.Commands;
using ColabSpace.WebAPI.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Planner
{
    public class CreatePlanner : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreatePlanner(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequest_ShouldCreatePlanner()
        {

            var client = await _factory.GetAuthenticatedClientAsync();

            List<TagModel> listTags = new List<TagModel>();

            listTags.AddRange(new[] {
                new TagModel
                { TagName = "Tag name 1"},
                new TagModel
                { TagName = "Tag name 2"}
            });
            var command = new CreatePlannerCommand
            {
                Id = Guid.NewGuid(),
                Purpose = "Test Purpose",
                Tags = listTags,
                Title = "Title Planner 1",
            };

            var content = IntegrationTestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/Planner", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
