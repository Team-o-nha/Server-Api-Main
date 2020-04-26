//using ColabSpace.Application.HelpItems.Commands.CreateHelpItem;
//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Application.TaskItems.Models;
//using ColabSpace.Application.UnitTests.Common;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Commands.CreateHelpItem
//{
//    public class CreateHelpItemCommandHandlerTests : CommandTestBase
//    {
//        [Fact]
//        public async Task Handle_GivenValidRequest_ShouldRaiseHelpItemCreatedNotificationAsync()
//        {
//            // Arrange
//            var sut = new CreateHelpItemCommandHandler(_context);

//            var command = new CreateHelpItemCommand()
//            {
//                Name = "About",
//                Description = string.Empty,
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                },
//            };

//            // Act
//            var resultGuid = await sut.Handle(command, CancellationToken.None);
//            var resulEntity = _context.HelpItems.Find(resultGuid);

//            // Assert
//            resulEntity.ShouldNotBeNull();

//            resulEntity.Name.ShouldBe(command.Name);
//            resulEntity.Description.ShouldBe(command.Description);
//            resulEntity.Content.ShouldNotBeNull();
//        }
//    }
//}
