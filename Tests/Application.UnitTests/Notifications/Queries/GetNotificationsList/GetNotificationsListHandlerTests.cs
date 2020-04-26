using AutoMapper;
using ColabSpace.Application.Notifications.Queries.GetNotificationsList;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Domain.Entities;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Notifications.Queries.GetNotificationsList
{
    [Collection("QueryCollection")]
    public class GetNotificationsListHandlerTests
    {
        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid emptyConversationUserId = Guid.NewGuid();
        private readonly Guid validUserId2 = ColabSpaceDbContextFactory.userId2;

        public GetNotificationsListHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNotifications_ListIsEmpty_Test()
        {
            var sut = new GetNotificationsByUserIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByUserIdQuery
            {
                UserId = emptyConversationUserId
            }
            , CancellationToken.None);

            result.Notifications.Count().ShouldBe(0);
        }

        [Fact]
        public async Task GiveValidUserId_GetListNotifications_Test()
        {
            var sut = new GetNotificationsByUserIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByUserIdQuery
            {
                UserId = validUserId2
            }
            , CancellationToken.None);

            result.Notifications.Count().ShouldNotBe(0);
        }

    }
}
