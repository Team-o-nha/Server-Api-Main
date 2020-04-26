using ColabSpace.Application.MessageChats.Commands.CreateMessageChat;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.CreateMessageChat
{
    public class CreateMessageChatsCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void Validate_ContentExceedMaximunLength_ShouldBeFalse()
        {
            var command = new CreateMessageChatCommand
            {
                Content = new string('a', 5001),
                ConversationId = Guid.NewGuid(),
                RegUserName = "user name"
            };

            var validator = new CreateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ConversationIdEmpty_ShouldBeFalse()
        {
            var command = new CreateMessageChatCommand
            {
                Content = "Hello there",
                ConversationId = Guid.Empty,
                RegUserName = "user name"
            };

            var validator = new CreateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_RegUserNameEmpty_ShouldBeFalse()
        {
            var command = new CreateMessageChatCommand
            {
                Content = "Hello there",
                ConversationId = Guid.Empty,
                RegUserName = string.Empty
            };

            var validator = new CreateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }        

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new CreateMessageChatCommand
            {
                Content = "Hello there",
                ConversationId = Guid.NewGuid(),
                RegUserName = "user name"
            };

            var validator = new CreateMessageChatCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
