using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.PinAttachFile
{
    public class PinAttachFileCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void Validate_MessageIdEmpty_ShouldBeFalse()
        {
            var command = new PinAttachFileCommand
            {
                MessageId = Guid.Empty,
                BlobStorageUrl = "https://domain.com/blob-container/kxg5qfz4.jxl",
                IsPinFile = true
            };

            var validator = new PinAttachFileCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_BlobStorageUrlEmptyOrNull_ShouldBeFalse()
        {
            var command = new PinAttachFileCommand
            {
                MessageId = Guid.NewGuid(),
                BlobStorageUrl = null,
                IsPinFile = true
            };

            var validator = new PinAttachFileCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldBeTrue()
        {
            var command = new PinAttachFileCommand
            {
                MessageId = Guid.NewGuid(),
                BlobStorageUrl = "https://domain.com/blob-container/kxg5qfz4.jxl",
                IsPinFile = true
            };

            var validator = new PinAttachFileCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }
    }
}
