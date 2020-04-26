using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandTests : CommandTestBase
    {
        private readonly Guid validUserId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid validUserId2 = ColabSpaceDbContextFactory.userId2;

        /**
         * Given type invalid throws NotTypeException
         */
        [Fact]
        public async Task Handle_GivenTypeInvalid_ShouldRaiseNotTypeException()
        {
            ////Arrange
            var sut = new CreateConversationCommandHandler(_context, _mapper);

            var command = new CreateConversationCommand
            {
                Type = "invalid",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        DisplayName = "Valid User 1",
                        UserId = validUserId1
                    },
                    new UserModel
                    {
                        DisplayName = "Valid User 2",
                        UserId = validUserId2
                    },
                }
            };

            //// Act
            await Assert.ThrowsAsync<NotTypeException>(() => sut.Handle(command, CancellationToken.None));
        }

        /**
         * Given valid request
         * Create conversation success
         */
        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateConversationSuccess()
        {
            ////Arrange
            var sut = new CreateConversationCommandHandler(_context, _mapper);

            var command = new CreateConversationCommand
            {
                Name = "conversationName",
                Type = "pair",
                Members = new List<UserModel>
                {
                    new UserModel
                    {
                        DisplayName = "Valid User 1",
                        UserId = validUserId1
                    },
                    new UserModel
                    {
                        DisplayName = "Valid User 2",
                        UserId = validUserId2
                    },
                },
                IsPublic = true
            };

            //// Act
            var result = await sut.Handle(command, CancellationToken.None);

            var entity = _context.Conversations.Find(result.Id);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe("conversationName");
            entity.Type.ShouldBe("pair");
            entity.Members.Count.ShouldBe(2);
            entity.isPublic.ShouldBe(true);
        }
    }
}
