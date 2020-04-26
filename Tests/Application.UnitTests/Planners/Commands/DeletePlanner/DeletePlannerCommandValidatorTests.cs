using ColabSpace.Application.Planners.Commands.DeletePlanner;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenIdIsNotNull()
        {
            var command = new DeletePlannerCommand
            {
                Id = Guid.NewGuid(),
            };

            var validator = new DeletePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
        [Fact]
        public void IsInValid_ShouldBeTrue_WhenIdIsNull()
        {
            var command = new DeletePlannerCommand
            {
            };

            var validator = new DeletePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
        [Fact]
        public void IsInValid_ShouldBeTrue_WhenIdIsGuidEmty()
        {
            var command = new DeletePlannerCommand
            {
                Id = Guid.Empty,
            };

            var validator = new DeletePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
