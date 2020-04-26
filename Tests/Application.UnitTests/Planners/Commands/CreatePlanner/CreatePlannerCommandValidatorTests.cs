using ColabSpace.Application.Planners.Commands;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Commands.CreatePlanner
{
    public class CreatePlannerCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenTitleIsNotNull()
        {
            var command = new CreatePlannerCommand
            {
                Title = "Not Null < 200 byte",
                Id = Guid.NewGuid(),
            };

            var validator = new CreatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }


        [Fact]
        public void IsValid_ShouldBeFalse_WhenIdIsNull()
        {
            var command = new CreatePlannerCommand
            {
                Title = "Not Null < 200 byte",
            };

            var validator = new CreatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenTitleIsNull()
        {
            var command = new CreatePlannerCommand
            {
                Title = null,
                Id = Guid.NewGuid(),
            };

            var validator = new CreatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenTitleIsGreaterThan200Byte()
        {
            var command = new CreatePlannerCommand
            {
                Title = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1",
                Id = Guid.NewGuid(),
            };

            var validator = new CreatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
