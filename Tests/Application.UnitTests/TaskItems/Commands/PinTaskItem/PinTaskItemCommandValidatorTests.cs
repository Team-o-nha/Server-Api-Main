using ColabSpace.Application.TaskItems.Commands.PinTaskItem;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.PinTaskItem
{
    public class PinTaskItemCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeFalse_WhenTaskIdIsEmpty()
        {
            var query = new PinTaskItemCommand
            {
                Id = Guid.Empty
            };

            var validator = new PinTaskItemCommandValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeTrue_CommandValid()
        {
            var query = new PinTaskItemCommand
            {
                Id = Guid.NewGuid()
            };

            var validator = new PinTaskItemCommandValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(true);
        }
    }
}
