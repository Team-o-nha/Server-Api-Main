using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.UpdateMessageChat
{
    public class UpdateMessageChatCommandValidatorTests : CommandTestBase
    {

        [Fact]
        public void Validate_ConversationIdEmpty_ShouldBeFalse()
        {
            var command = new UpdateMessageChatCommand
            {
                MessageId = Guid.Empty,
            };

            var validator = new UpdateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new UpdateMessageChatCommand
            {
                MessageId = Guid.NewGuid(),
            };

            var validator = new UpdateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
