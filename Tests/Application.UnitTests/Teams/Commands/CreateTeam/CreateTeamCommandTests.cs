using ColabSpace.Application.Teams.Commands.CreateTeam;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Application.UnitTests.Common;
using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidRequest_ShouldRaiseTeamCreatedNotificationAsync()
        {
            // Arrange
            var sut = new CreateTeamCommandHandler(_context);

            var teamName = "Team1";
            var users = new List<UserModel>
            {
                new UserModel()
                {
                    UserId = Guid.NewGuid(),
                    DisplayName = "TestUser1"
                }
            };
            var command = new CreateTeamCommand { Name = teamName, Users = users, Description = "Des1" };

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            var entity = _context.Teams.Find(result);

            var conversationEntity = await _context.Conversations.Where(t => t.TeamId == entity.Id.ToString()).ToListAsync();
            var generalChannel = conversationEntity.First();

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(teamName);
            conversationEntity.Count.ShouldBe(1);
            generalChannel.Type.ShouldBe("channel");
            generalChannel.Name.ShouldBe("General");
            generalChannel.isPublic.ShouldBeTrue();
            generalChannel.Members.Count.ShouldBe(entity.Users.Count);
            foreach (User channelMember in generalChannel.Members)
            {
                entity.Users.First(t => t.UserOid == channelMember.UserOid).ShouldNotBeNull();
            }
        }
    }
}
