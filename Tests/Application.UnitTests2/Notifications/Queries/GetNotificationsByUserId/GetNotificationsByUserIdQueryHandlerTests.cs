using AutoMapper;
using ColabSpace.Application.Notifications.Queries.GetNotificationsList;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Notifications.Queries.GetNotificationsByUserId
{
    [Collection("QueryCollection")]
    public class GetNotificationsByUserIdQueryHandlerTests
    {
        private readonly Guid userInOneTeam = ColabSpaceDbContextFactory.userId1;
        private readonly Guid teamId1 = ColabSpaceDbContextFactory.teamId1;

        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetNotificationsByUserIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_GivenUserNotInAnyTeam_ReturnListEmpty()
        {
            var sut = new GetNotificationsByUserIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByUserIdQuery
            {
                UserId = Guid.NewGuid(),
                TeamId = teamId1.ToString()
            }
            , CancellationToken.None);

            result.Notifications.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenUserOneTeam_ReturnListOne()
        {
            var sut = new GetNotificationsByUserIdQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByUserIdQuery
            {
                UserId = userInOneTeam,
                TeamId = teamId1.ToString()
            }
            , CancellationToken.None);

            result.Notifications.Count().ShouldBe(1);
        }
    }
}
