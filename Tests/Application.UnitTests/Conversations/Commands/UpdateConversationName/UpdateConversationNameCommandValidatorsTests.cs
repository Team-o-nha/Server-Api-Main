using ColabSpace.Application.Conversations.Commands.UpdateConversationName;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.UpdateConversationName
{
    public class UpdateConversationNameCommandValidatorsTests : CommandTestBase
    {
        private readonly Guid validConversationId1 = ColabSpaceDbContextFactory.conversation1;

        [Fact]
        public void Validate_NameIsGreaterThan200_ShouldBeFalse()
        {
            var command = new UpdateConversationNameCommand
            {
                Name = new string('a', 201),
                Id = validConversationId1
            };

            var validator = new UpdateConversationNameCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new UpdateConversationNameCommand
            {
                Name = new string('a', 200),
                Id = validConversationId1
            };

            var validator = new UpdateConversationNameCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
