//using ColabSpace.Application.HelpItems.Commands.UpdateHelpItem;
//using ColabSpace.Application.TaskItems.Models;
//using ColabSpace.Application.UnitTests.Common;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Commands.UpdateHelpItem
//{
//    public class UpdateHelpItemCommandValidatorTests : CommandTestBase
//    {
//        [Fact]
//        public void IsValid_ShouldBeFalse_WhenNameIsEmpty()
//        {
//            var command = new UpdateHelpItemCommand
//            {
//                Name = string.Empty
//            };

//            var validator = new UpdateHelpItemCommandValidator();
//            var result = validator.Validate(command);

//            result.IsValid.ShouldBe(false);
//        }

//        [Fact]
//        public void IsValid_ShouldBeFalse_WhenNameExceedingMaxLength()
//        {
//            var command = new UpdateHelpItemCommand
//            {
//                Name = new string('a', 21)
//            };

//            var validator = new UpdateHelpItemCommandValidator();
//            var result = validator.Validate(command);

//            result.IsValid.ShouldBe(false);
//        }

//        [Fact]
//        public void IsValid_ShouldBeFalse_WhenDescriptionExceedingMaxLength()
//        {
//            var command = new UpdateHelpItemCommand
//            {
//                Name = new string('a', 20),
//                Description = new string('a', 501),
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            var validator = new UpdateHelpItemCommandValidator();
//            var result = validator.Validate(command);

//            result.IsValid.ShouldBe(false);
//        }

//        [Fact]
//        public void IsValid_ShouldBeFalse_WhenContentIsEmpty()
//        {
//            var command = new UpdateHelpItemCommand
//            {
//                Name = new string('a', 20),
//                Content = null
//            };

//            var validator = new UpdateHelpItemCommandValidator();
//            var result = validator.Validate(command);

//            result.IsValid.ShouldBe(false);
//        }

//        [Fact]
//        public void IsValid_ShouldBeTrue_WhenCommandIsValid()
//        {
//            var command = new UpdateHelpItemCommand
//            {
//                Name = new string('a', 20),
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            var validator = new UpdateHelpItemCommandValidator();
//            var result = validator.Validate(command);

//            result.IsValid.ShouldBe(true);
//        }
//    }
//}
