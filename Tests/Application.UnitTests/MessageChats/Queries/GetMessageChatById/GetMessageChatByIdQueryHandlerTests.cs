using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatById;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Queries.GetMessageChatById
{
    [Collection("QueryCollection")]
    public class GetMessageChatByIdQueryHandlerTests
    {
        private readonly Guid validMessageChatId = ColabSpaceDbContextFactory.messagechannelId1;

        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetMessageChatByIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_InvalidMessageChatId_NotFound()
        {
            var sut = new GetMessageChatByIdQueryHandler(_context, _mapper);

            var query = new GetMessageChatByIdQuery
            {
                MessageChatId = Guid.NewGuid()
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidMessageChatId_NotFound()
        {
            var sut = new GetMessageChatByIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetMessageChatByIdQuery
            {
                MessageChatId = validMessageChatId
            }
            , CancellationToken.None);

            result.ShouldBeOfType<MessageChatModel>();
            result.Id.ShouldBe(validMessageChatId);
        }
    }
}
