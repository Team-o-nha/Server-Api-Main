using AutoMapper;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatList;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Queries.GetMessageChatsList
{
    [Collection("QueryCollection")]
    public class GetMessageChatsQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid emptyMessage_ConversationId = Guid.NewGuid();
        private readonly Guid oneMessage_ConversationId = ColabSpaceDbContextFactory.conversation2;
        private readonly Guid twoMessage_ConversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid emptyMessage_RelatedMessageId = Guid.NewGuid();
        private readonly Guid oneMessage_RelatedMessageId = ColabSpaceDbContextFactory.relatedMessageId1;
        private readonly Guid twoMessage_RelatedMessageId = ColabSpaceDbContextFactory.relatedMessageId2;

        public GetMessageChatsQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_GivenConversationIdHaveNoMessage_ReturnListEmpty()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                ConversationId = emptyMessage_ConversationId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenConversationIdHaveOneMessage_ReturnListMessage()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                ConversationId = oneMessage_ConversationId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Handle_GivenConversationIdHaveTwoMessage_ReturnListMessage()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                ConversationId = twoMessage_ConversationId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Handle_GivenConversationIdHaveTwoMessage_ReturnListMessage_WithPageIndex_ReturnListMessage()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                ConversationId = twoMessage_ConversationId,
                PageIndex = 1
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Handle_GivenRelatedMessageIdHaveNoMessage_ReturnListEmpty()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                RelatedMessagesId = emptyMessage_RelatedMessageId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenRelatedMessageIdHaveOneMessage_ReturnListMessage()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                RelatedMessagesId = oneMessage_RelatedMessageId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Handle_GivenRelatedMessageIdHaveTwoMessage_ReturnListMessage()
        {
            var sut = new GetMessageChatsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatsQuery
            {
                RelatedMessagesId = twoMessage_RelatedMessageId
            }
            , CancellationToken.None);

            result.Count().ShouldBe(2);
        }


    }
}
