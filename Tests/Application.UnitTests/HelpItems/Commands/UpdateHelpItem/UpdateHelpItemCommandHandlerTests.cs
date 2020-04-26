//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.HelpItems.Commands.UpdateHelpItem;
//using ColabSpace.Application.TaskItems.Models;
//using ColabSpace.Application.UnitTests.Common;
//using ColabSpace.Domain.Entities;
//using MediatR;
//using Moq;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Commands.UpdateHelpItem
//{
//    public class UpdateHelpItemCommandHandlerTests : CommandTestBase
//    {
//        private readonly Guid validHelpItemId = new Guid("63E5177F-E963-4ED3-A3A5-E1E968B8D0B2");
//        private readonly Guid invalidHelpItemId = Guid.NewGuid();

//        public UpdateHelpItemCommandHandlerTests()
//        {
//            _context.HelpItems.AddRange(new[] {
//                new HelpItem()
//                {
//                    Id = validHelpItemId,
//                    Name = "Topic",
//                    Description = string.Empty,
//                    Content = new AttachFile()
//                    {
//                        FileName = "test.a",
//                        FileSize = 10,
//                        BlobStorageUrl = null,
//                        FileStorageName = "test.b",
//                    },
//                }
//            });
//            _context.SaveChanges();
//        }

//        [Fact]
//        public async Task Handle_GivenInvalidId_ShouldRaiseException()
//        {
//            // Arrange
//            var sut = new UpdateHelpItemCommandHandler(_context);

//            // Act
//            var command = new UpdateHelpItemCommand
//            {
//                Id = invalidHelpItemId,
//                Name = "help item name",
//                Description = "help item description",
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            // Assert
//            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
//        }

//        [Fact]
//        public async Task Handle_GivenValidRequest_ShouldUpdateHelpItem()
//        {
//            // Arrange
//            var sut = new UpdateHelpItemCommandHandler(_context);

//            // Act
//            var command = new UpdateHelpItemCommand
//            {
//                Id = validHelpItemId,
//                Name = "new help item name",
//                Description = "new help item description",
//                Content = new AttachFileModel()
//                {
//                    FileName = "test.a",
//                    FileSize = 10,
//                    BlobStorageUrl = null,
//                    FileStorageName = "test.b",
//                }
//            };

//            await sut.Handle(command, CancellationToken.None);

//            var entity = _context.HelpItems.Find(validHelpItemId);

//            entity.ShouldNotBeNull();
//            entity.Name.ShouldBe(command.Name);
//            entity.Description.ShouldBe(command.Description);
//            entity.Content.ShouldNotBeNull();
//        }
//    }
//}
