using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using ColabSpace.Domain.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Infrastructure.IntegrationTests.Persistence
{
    public class ColabSpaceDbContextTests : IDisposable
    {
        private readonly string _userId;
        private readonly DateTime _dateTime;
        private readonly Mock<IDateTime> _dateTimeMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ColabSpaceDbContext _sut;
        private readonly Guid teamId = Guid.NewGuid();

        public ColabSpaceDbContextTests()
        {
            _dateTime = new DateTime(3001, 1, 1);
            _dateTimeMock = new Mock<IDateTime>();
            _dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

            _userId = "00000000-0000-0000-0000-000000000000";
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(m => m.UserId).Returns(_userId);

            var options = new DbContextOptionsBuilder<ColabSpaceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _sut = new ColabSpaceDbContext(options, _currentUserServiceMock.Object, _dateTimeMock.Object);

            var users = new List<User>
            {
                new User()
                {
                    UserOid = _userId,
                    DisplayName = "TestUser1"
                }
            };

            _sut.Teams.Add(new Team
            {
                Id = teamId,
                Name = "Team1",
                Users = users
            });

            _sut.SaveChanges();
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewTeam_ShouldSetCreatedProperties()
        {
            var users = new List<User>
            {
                new User()
                {
                    UserOid = _userId,
                    DisplayName = "TestUser1"
                }
            };

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Team2",
                Users = users
            };

            _sut.Teams.Add(team);

            await _sut.SaveChangesAsync();

            team.Created.ShouldBe(_dateTime);
            team.CreatedBy.ShouldBe(_userId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingTeam_ShouldSetLastModifiedProperties()
        {
            var team = await _sut.Teams.FindAsync(teamId);
            
            var users = new List<User>
            {
                new User()
                {
                    UserOid = "11111111-1111-1111-1111-111111111",
                    DisplayName = "TestUser2"
                }
            };

            team.Description = "Description 2";
            team.Users = users;

            await _sut.SaveChangesAsync();

            team.LastModified.ShouldNotBeNull();
            team.LastModified.ShouldBe(_dateTime);
            team.LastModifiedBy.ShouldBe(_userId);
        }


        public void Dispose()
        {
            _sut?.Dispose();
        }
    }
}
