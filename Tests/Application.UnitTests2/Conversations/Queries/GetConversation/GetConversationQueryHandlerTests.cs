using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Conversations.Queries.GetConversation;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Conversations.Queries.GetConversation
{
    [Collection("QueryCollection2")]
    public class GetConversationQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid userId2 = ColabSpaceDbContextFactory.userId2;
        private readonly Guid conversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid InvalidConversationId = Guid.NewGuid();
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public GetConversationQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            // Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
        }

        [Fact]
        public async Task GiveValidId_ShouldRaiseConversation()
        {
            var sut = new GetConversationQueryHandler(_context, _mapper);
            var result = await sut.Handle(new GetConversationQuery
            {
                ConversationId = conversationId.ToString(),
                LoginUserId = _currentUserServiceMock.Object.UserId
            }, CancellationToken.None);

            result.ShouldBeOfType<ConversationModel>();
            result.Id.ShouldBe(conversationId);
        }

        [Fact]
        public async Task GiveInvalidId_ShouldRaiseNotFound()
        {
            var sut = new GetConversationQueryHandler(_context, _mapper);
            await Should.ThrowAsync<NotFoundException>(() =>
               sut.Handle(new GetConversationQuery
               {
                   ConversationId = InvalidConversationId.ToString(),
                   LoginUserId = _currentUserServiceMock.Object.UserId
               }, CancellationToken.None));
        }

        [Fact]
        public async Task GiveValidMemberIds_ShouldRaiseConversation()
        {
            var sut = new GetConversationQueryHandler(_context, _mapper);
            List<string> memberList = new List<string>();
            memberList.Add(userId1.ToString());
            memberList.Add(userId2.ToString());

            var result = await sut.Handle(new GetConversationQuery
            {
                MembersId = memberList,
                LoginUserId = _currentUserServiceMock.Object.UserId
            }, CancellationToken.None);

            result.ShouldBeOfType<ConversationModel>();
            result.Id.ShouldBe(conversationId);
            result.Type.ShouldBe("pair");
        }
    }
}
