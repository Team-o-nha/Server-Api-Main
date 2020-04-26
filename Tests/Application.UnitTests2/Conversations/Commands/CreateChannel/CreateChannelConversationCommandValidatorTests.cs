using ColabSpace.Application.Conversations.Commands.CreateChannelConversation;
using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Conversations.Commands.CreateChannel
{
    [Collection("QueryCollection")]
    public class CreateChannelConversationCommandValidatorTests
    {
        private readonly Guid teamId1 = ColabSpaceDbContextFactory.teamId1;

        [Fact]
        public void Validate_MemberIsEmpty_IsPublic_ShouldBeTrue()
        {
            var command = new CreateChannelConversationCommand
            {
                Members = new List<UserModel>() { },
                IsPublic = true,
                Name = "TEST isPublic Channel",
                TeamId = teamId1.ToString()
            };

            var validator = new CreateChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Validate_MemberIsEmpty_IsNOTPublic_ShouldBeFalse()
        {
            var command = new CreateChannelConversationCommand
            {
                Members = new List<UserModel>() { },
                IsPublic = true,
                Name = "TEST isPublic Channel",
                TeamId = teamId1.ToString()
            };

            var validator = new CreateChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Validate_MemberIsLessThanTwo_ShouldBeFalse()
        {
            var command = new CreateChannelConversationCommand
            {
                Members = new List<UserModel> { },
                TeamId = teamId1.ToString(),
                IsPublic = true
            };

            var validator = new CreateChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_NameIsGreaterThan200_ShouldBeFalse()
        {
            var command = new CreateChannelConversationCommand
            {
                Name = new string('a', 201),
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
                },
                TeamId = teamId1.ToString(),
                IsPublic = false
            };

            var validator = new CreateChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new CreateChannelConversationCommand
            {
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
                },
                TeamId = teamId1.ToString(),
                IsPublic = false,
                Name = "Test channel ..",
            };

            var validator = new CreateChannelConversationCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
