using ColabSpace.Application.Conversations.Commands.DeleteChannelConversation;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.DeleteChannelConversation
{
    public class DeleteChannelConversationCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void Validate_ConversationIdIsEmpty_ShouldBeFalse()
        {
            var command = new DeleteChannelConversationCommand
            {
                ConversationId = string.Empty
            };

            var validator = new DeleteChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new DeleteChannelConversationCommand
            {
                ConversationId = Guid.NewGuid().ToString()
            };

            var validator = new DeleteChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
