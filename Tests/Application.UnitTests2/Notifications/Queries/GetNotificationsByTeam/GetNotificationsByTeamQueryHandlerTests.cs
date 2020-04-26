using AutoMapper;
using ColabSpace.Application.Notifications.Queries.GetNotificationsByTeam;
using ColabSpace.Application.UnitTests2.Common;
using ColabSpace.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests2.Notifications.Queries.GetNotificationsByTeam
{
    [Collection("QueryCollection")]
    public class GetNotificationsByTeamQueryHandlerTests
    {
        private readonly Guid userInOneTeam = ColabSpaceDbContextFactory.userId1;

        private readonly ColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetNotificationsByTeamQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_GivenUserNotInAnyTeam_ReturnListEmpty()
        {
            var sut = new GetNotificationsByTeamQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByTeamQuery
            {
                UserId = Guid.NewGuid()
            }
            , CancellationToken.None);

            result.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Handle_GivenUserOneTeam_ReturnListOne()
        {
            var sut = new GetNotificationsByTeamQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetNotificationsByTeamQuery
            {
                UserId = userInOneTeam
            }
            , CancellationToken.None);

            result.Count().ShouldBe(1);
        }
    }
}
