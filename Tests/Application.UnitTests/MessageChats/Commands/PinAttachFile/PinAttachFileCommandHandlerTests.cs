using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Commands.PinAttachFile
{
    public class PinAttachFileCommandHandlerTests : CommandTestBase
    {
        private readonly Guid validMessageId = ColabSpaceDbContextFactory.messageId1;
        private readonly string validBlobStorageUrl = ColabSpaceDbContextFactory.validBlobStorageUrl;

        [Fact]
        public async Task Handle_GivenInvalidMessageId_ShouldRaiseException()
        {
            //// Arrange
            var sut = new PinAttachFileCommandHandler(_context);

            var command = new PinAttachFileCommand
            {
                MessageId = Guid.NewGuid(),
                BlobStorageUrl = "some-url.com",
                IsPinFile = true
            };

            //// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenInvalidBlobStorageUrl_ShouldRaiseException()
        {
            //// Arrange
            var sut = new PinAttachFileCommandHandler(_context);

            var command = new PinAttachFileCommand
            {
                MessageId = validMessageId,
                BlobStorageUrl = "invalid-url/invalid-file-name",
                IsPinFile = true
            };

            //// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Validate_ValidCommand_ShouldBeTrueAsync()
        {
            //// Arrange
            var sut = new PinAttachFileCommandHandler(_context);

            var command = new PinAttachFileCommand
            {
                MessageId = validMessageId,
                BlobStorageUrl = validBlobStorageUrl,
                IsPinFile = true
            };

            /// Act
            await sut.Handle(command, CancellationToken.None);

            var entity = _context.MessageChats.Find(validMessageId);
            entity.ShouldNotBeNull();

            var attachFile = entity.AttachFileList
                .Where(file => file.BlobStorageUrl == validBlobStorageUrl)
                .FirstOrDefault();

            attachFile.IsPin.ShouldBe(true);
        }
    }
}
