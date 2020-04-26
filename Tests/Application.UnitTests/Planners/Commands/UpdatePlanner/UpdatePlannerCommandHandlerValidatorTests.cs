using ColabSpace.Application.Planners.Commands.Update;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Commands.UpdatePlanner
{
    public class UpdatePlannerCommandHandlerValidatorTests : CommandTestBase
    {
        private readonly Guid validPlannerId = ColabSpaceDbContextFactory.plannerId1;
        [Fact]
        public void IsValid_ShouldBeTrue_WhenTitleLessThan200byte()
        {
            var command = new UpdatePlannerCommand
            {
                Title = "Not Null < 200 byte",
                Id = validPlannerId,
            };

            var validator = new UpdatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }


        [Fact]
        public void IsValid_ShouldBeFalse_WhenIdIsNull()
        {
            var command = new UpdatePlannerCommand
            {
                Title = "Not Null < 200 byte",
            };

            var validator = new UpdatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenTitleIsGreaterThan200Byte()
        {
            var command = new UpdatePlannerCommand
            {
                Title = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1",
                Id = validPlannerId,
            };

            var validator = new UpdatePlannerCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
