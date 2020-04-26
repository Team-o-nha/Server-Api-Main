//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.HelpItems.Commands.DeleteHelpItem;
//using ColabSpace.Application.UnitTests.Common;
//using ColabSpace.Domain.Entities;
//using Shouldly;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Commands.DeleteHellpItem
//{
//    public class DeleteHelpItemCommandHandlerTests : CommandTestBase
//    {
//        private readonly Guid validHelpItemId = new Guid("63E5177F-E963-4ED3-A3A5-E1E968B8D0B2");

//        public DeleteHelpItemCommandHandlerTests()
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
//        public async Task Handle_GivenInvalidId_ThrowsNotFoundException()
//        {
//            var invalidId = Guid.NewGuid();

//            var command = new DeleteHelpItemCommand { Id = invalidId };

//            var sut = new DeleteHelpItemCommandHandler(_context);

//            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
//        }

//        [Fact]
//        public async Task Handle_GivenValidId_AssertDelete()
//        {
//            var command = new DeleteHelpItemCommand { Id = validHelpItemId };

//            var sut = new DeleteHelpItemCommandHandler(_context);
//            await sut.Handle(command, CancellationToken.None);

//            var entity = _context.HelpItems.Find(validHelpItemId);

//            Assert.Null(entity);
//        }
//    }
//}
