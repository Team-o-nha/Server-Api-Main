using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Planners.Commands.DeletePlanner;
using ColabSpace.Application.TaskItems.Commands.DeleteTaskItem;
using ColabSpace.Application.UnitTests.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandTests : CommandTestBase
    {
        private readonly Guid validPlannerId = ColabSpaceDbContextFactory.plannerId1;

        /**
         * Given planner does not exist throws NotFoundException
         */
        [Fact]
        public async Task Handle_GivenInvalidPlannerId_ThrowsNotFoundException()
        {
            // Login user is creator
            var _sut = new DeletePlannerCommandHandler(_context, _mapper);

            var invalidPlannerId = Guid.NewGuid();

            var command = new DeletePlannerCommand { Id = invalidPlannerId };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given valid planner id 
         * delete success
         */
        [Fact]
        public async Task Handle_GivenValidPlannerId_DeleteSuccess()
        {
            // Login user is creator
            var _sut = new DeletePlannerCommandHandler(_context, _mapper);

            var command = new DeletePlannerCommand { Id = validPlannerId };

            await _sut.Handle(command, CancellationToken.None);

            var planner = await _context.TaskItems.FindAsync(validPlannerId);

            Assert.Null(planner);
        }
    }
}
