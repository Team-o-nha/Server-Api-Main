using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Planners.Commands.Update;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using ColabSpace.WebAPI.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests.Controllers.Planner
{
    public class UpdatePlanner : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdatePlanner(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidUpdateTaskItemCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdatePlannerCommand
            {
                Id = Guid.Parse("CBB85A08-ED54-4924-9135-E1F723A2BA6B"),
                Title = "Updated",
                Purpose = "Updated",
                Milestones = new List<MilestoneModel>
                {
                    new MilestoneModel()
                    {
                        Title = "Milestone 1",
                        Description = "Milestone 1 Description",
                        Date = DateTime.UtcNow,
                        Tasks = new List<TaskItemModel>
                        {
                            new TaskItemModel()
                            {
                                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae")
                            },
                            new TaskItemModel()
                            {
                                Id = new Guid("de14a885-71d4-4da0-bb17-048d74d33ada")
                            }
                        }
                    }
                },
                Tags = new List<TagModel>
                {
                    new TagModel()
                    {
                        TagName = "Tag 4"
                    },
                    new TagModel()
                    {
                        TagName = "Tag 5"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/Planner", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldRaiseNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdatePlannerCommand
            {
                Id = Guid.NewGuid(),
                Title = "Updated",
                Purpose = "Updated",
                Milestones = new List<MilestoneModel>
                {
                    new MilestoneModel()
                    {
                        Title = "Milestone 1",
                        Description = "Milestone 1 Description",
                        Date = DateTime.UtcNow,
                        Tasks = new List<TaskItemModel>
                        {
                            new TaskItemModel()
                            {
                                Id = new Guid("197d0438-e04b-453d-b5de-eca05960c6ae")
                            },
                            new TaskItemModel()
                            {
                                Id = new Guid("de14a885-71d4-4da0-bb17-048d74d33ada")
                            }
                        }
                    }
                },
                Tags = new List<TagModel>
                {
                    new TagModel()
                    {
                        TagName = "Tag 4"
                    },
                    new TagModel()
                    {
                        TagName = "Tag 5"
                    }
                }
            };

            var content = IntegrationTestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/Planner", content);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}


