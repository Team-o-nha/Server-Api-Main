using ColabSpace.Application.UnitTests.Common;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Threading;
using System.Linq;
using ColabSpace.Application.Planners.Commands;
using ColabSpace.Domain.Entities;
using ColabSpace.Application.Common.Models;
using System.Collections.Generic;

namespace ColabSpace.Application.UnitTests.Planners.Commands.CreatePlanner
{
    public class CreatePlannerCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreatePlanner()
        {
            // Arrange
            var sut = new CreatePlannerCommandHandler(_context, _mapper);
            List<TagModel> lstTag = new List<TagModel> { new TagModel() { TagName = "Test" }, new TagModel() { TagName = "Test2" } };

            var command = new CreatePlannerCommand()
            {
                Id = Guid.NewGuid(),
                Purpose = "Test purpose",
                Tags = lstTag,
                Title = "Test title",
            };

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            var entity = _context.Planners.Find(result.Id);

            entity.ShouldNotBeNull();
            entity.Title.ShouldBe("Test title");
            entity.Purpose.ShouldBe("Test purpose");
            entity.Tags.Count().ShouldBe(2);
            lstTag.ForEach(p => entity.Tags.Any(t => t.TagName == p.TagName).ShouldBeTrue());
        }
    }
}

