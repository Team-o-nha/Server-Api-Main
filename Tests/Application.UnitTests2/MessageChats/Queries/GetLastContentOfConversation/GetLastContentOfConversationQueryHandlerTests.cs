using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.MessageChats.Queries.GetLastContentOfConversation;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.MessageChats.Queries.GetLastContentOfConversation
{
    [Collection("QueryCollection2")]
    public class GetLastContentOfConversationQueryHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid validTeamId = ColabSpaceDbContextFactory.teamId1;
        private readonly Guid userId1 = ColabSpaceDbContextFactory.userId1;
        private readonly Guid userId2 = ColabSpaceDbContextFactory.userId2;
        Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        public GetLastContentOfConversationQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            //// Login user
            _currentUserServiceMock.Setup(m => m.UserId)
                .Returns(userId1.ToString());
        }

        [Fact]
        public async Task GiveValidRequestWithTeamIdAndUserId_ShouldReturnChannelLastContentData()
        {
            var sut = new GetLastContentOfConversationQueryHandler(_context, _mapper, _currentUserServiceMock.Object);

            var command = new GetLastContentOfConversationQuery()
            {
                TeamId = validTeamId,
                UserId = Guid.Parse(_currentUserServiceMock.Object.UserId)
            };

            IEnumerable<ConversationLastContentModel> result = await sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldNotContain(x => x.Conversation.Type == "group");
            result.ShouldNotContain(x => x.Conversation.Type == "pair");
            result.Count().ShouldBe(3); // created general in mock data 2 -> 3 conversation
            result.ShouldContain(x => x.Conversation.Name == "Channel 1 Name");
            result.ShouldContain(x => x.LastMessageChatContent.Content == "message in channel 2"); // The last message
            result.ShouldContain(x => x.Conversation.Name == "Channel 2 Name");
            result.ShouldContain(x => x.LastMessageChatContent.Content == null); // Have no message inside
        }

        [Fact]
        public async Task GiveValidRequestWithNOTeamIdAndUserId_ShouldReturnConversationLastContentData()
        {
            var sut = new GetLastContentOfConversationQueryHandler(_context, _mapper, _currentUserServiceMock.Object);

            var command = new GetLastContentOfConversationQuery()
            {
                UserId = Guid.Parse(_currentUserServiceMock.Object.UserId)
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Count().ShouldBe(3);
            result.ShouldNotContain(x => x.Conversation.Type == "channel");

            result.ElementAt(0).Conversation.Type.ShouldBe("pair");
            result.ElementAt(0).LastMessageChatContent.Content.ShouldBe("message 2");

            result.ElementAt(1).Conversation.Type.ShouldBe("pair");
            result.ElementAt(1).LastMessageChatContent.Content.ShouldBe("message 3");

            result.ElementAt(2).Conversation.Type.ShouldBe("group");
            result.ElementAt(2).LastMessageChatContent.Content.ShouldBe("message in conversation 3: Group 3");
        }
    }
}
