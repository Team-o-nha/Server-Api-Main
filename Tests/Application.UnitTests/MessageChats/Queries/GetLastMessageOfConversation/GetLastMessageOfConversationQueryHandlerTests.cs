using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetLastMessageOfConversation;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Queries.GetLastMessageOfConversation
{
    [Collection("QueryCollection")]
    public class GetLastMessageOfConversationQueryHandlerTests
    {
        private readonly Guid twoMessage_ConversationId = ColabSpaceDbContextFactory.conversation1;
        private readonly Guid validMessageId = ColabSpaceDbContextFactory.messageId2;

        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetLastMessageOfConversationQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_ValidConversationId_Found()
        {
            var sut = new GetLastMessageOfConversationQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetLastMessageOfConversationQuery
            {
                ConversationId = twoMessage_ConversationId
            }
            , CancellationToken.None);

            result.ShouldBeOfType<MessageChatModel>();
            result.Id.ShouldBe(validMessageId);
        }
    }
}
