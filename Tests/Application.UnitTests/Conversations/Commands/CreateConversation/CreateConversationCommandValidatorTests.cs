using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void Validate_TypeIsEmpty_ShouldBeFalse()
        {
            var command = new CreateConversationCommand
            {
                Type = "",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Long"
                    },
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Quan"
                    },
                }
            };

            var validator = new CreateConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_MemberIsEmpty_ShouldBeFalse()
        {
            var command = new CreateConversationCommand
            {
                Type = "Is Not Empty",
                Members = new List<UserModel>
                {
                    
                }
            };

            var validator = new CreateConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_MemberIsLessThanTwo_ShouldBeFalse()
        {
            var command = new CreateConversationCommand
            {
                Type = "Is Not Empty",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Long"
                    }
                }
            };

            var validator = new CreateConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_NameIsGreaterThan200_ShouldBeFalse()
        {
            var command = new CreateConversationCommand
            {
                Name = new string('a', 201),
                Type = "Is Not Empty",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Long"
                    },
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Quan"
                    },
                }
            };

            var validator = new CreateConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new CreateConversationCommand
            {
                Type = "Is Not Empty",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Long"
                    },
                    new UserModel
                    {
                        UserId = Guid.NewGuid(),
                        DisplayName = "Quan"
                    },
                }
            };

            var validator = new CreateConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
