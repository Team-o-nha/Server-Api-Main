using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Conversations.Commands.UpdateConversationName;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.UpdateConversationName
{
    public class UpdateConversationNameCommandTests : CommandTestBase
    {
        private readonly Guid validConversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid validUserId1 = ColabSpaceDbContextFactory.userId1;

        [Fact]
        public async Task Handle_GivenConversationIdInvalid_ShouldRaiseNotFoundException()
        {
            ////Arrange
            var sut = new UpdateConversationNameCommandHandler(_context, _mapper);

            var command = new UpdateConversationNameCommand
            {
                Id = Guid.NewGuid(),
                Name = "valid group name"
            };

            //// Act
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GivenConversationIdValid_ShouldUpdateConversation()
        {
            ////Arrange
            var sut = new UpdateConversationNameCommandHandler(_context, _mapper);

            var command = new UpdateConversationNameCommand
            {
                Id = validConversationId,
                Name = "valid group name"
            };

            //// Act
            await sut.Handle(command, CancellationToken.None);

            var entity = _context.Conversations.Find(validConversationId);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe("valid group name");
        }

        [Fact]
        public async Task Handle_GivenConversationIdAndMemberListValid_ShouldUpdateConversation()
        {
            ////Arrange
            var sut = new UpdateConversationNameCommandHandler(_context, _mapper);

            var command = new UpdateConversationNameCommand
            {
                Id = validConversationId,
                Name = "valid group name",
                Members = new List<UserModel>()
                {
                    new UserModel()
                    {
                        UserId = validUserId1,
                        DisplayName = "test"
                    }
                }
            };

            //// Act
            await sut.Handle(command, CancellationToken.None);

            var entity = _context.Conversations.Find(validConversationId);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe("valid group name");
            entity.Members.Count.ShouldBe(1);
        }
    }
}
