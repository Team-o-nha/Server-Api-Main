using ColabSpace.Application.TaskItems.Commands.CreateTaskItem;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenNameIsNotNull()
        {
            var command = new CreateTaskItemCommand
            {
                Name = "Not Null < 200 byte"
            };

            var validator = new CreateTaskItemCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsNull()
        {
            var command = new CreateTaskItemCommand
            {
                Name = null
            };

            var validator = new CreateTaskItemCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsGreaterThan200Byte()
        {
            var command = new CreateTaskItemCommand
            {
                Name = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "1"
            };

            var validator = new CreateTaskItemCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}