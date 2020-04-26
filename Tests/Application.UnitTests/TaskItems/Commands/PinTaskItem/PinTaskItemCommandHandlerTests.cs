using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.TaskItems.Commands.PinTaskItem;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Commands.PinTaskItem
{
    public class PinTaskItemCommandHandlerTests : CommandTestBase
    {
        private readonly Guid validUnpinTaskItemId = ColabSpaceDbContextFactory.taskItemId3;

        [Fact]
        public async Task Handle_GivenInvalidTaskId_ShouldRaiseException()
        {
            /// Arrange
            var sut = new PinTaskItemCommandHandler(_context, _mapper);

            var command = new PinTaskItemCommand
            {
                Id = Guid.NewGuid(),
                IsPin = true
            };

            /// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenValidTaskId_ShouldUpdateTaskItem()
        {
            /// Arrange
            var sut = new PinTaskItemCommandHandler(_context, _mapper);

            var command = new PinTaskItemCommand
            {
                Id = validUnpinTaskItemId,
                IsPin = true
            };

            var result = await sut.Handle(command, CancellationToken.None);

            /// Assert
            result.IsPin.ShouldBe(true);
        }
    }
}
