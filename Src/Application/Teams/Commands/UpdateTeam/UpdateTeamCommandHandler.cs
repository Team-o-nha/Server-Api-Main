using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTeamCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams.FindAsync(Guid.Parse(request.Id));

            if (team == null)
            {
                throw new NotFoundException(nameof(Team), request.Id);
            }
            var loginUser = team.Users.First(u => u.UserOid == _currentUserService.UserId);
            if (loginUser.TeamRole != "Leader")
            {
                throw new NotOwnedException(nameof(Team), team.Id);
            }
            var users = new List<User>();
            foreach (UserModel userModel in request.Users)
            {
                users.Add(new User()
                {
                    DisplayName = userModel.DisplayName,
                    Email = userModel.Email,
                    Surname = userModel.Surname,
                    GivenName = userModel.GivenName,
                    UserPrincipalName = userModel.UserPrincipalName,
                    UserOid = userModel.UserId.ToString(),
                    TeamRole = userModel.TeamRole
                });

            }

            var deletedUsers = team.Users.Where(oldUser => !users.Any(newUser => newUser.UserOid == oldUser.UserOid)).ToList();

            // get value from request to update
            team.Name = request.Name;
            team.Description = request.Description;
            team.Users = users;

            // find all public channel in team
            var lstChannels = await _context.Conversations
                .Where(con => con.TeamId == team.Id.ToString())
                .ToListAsync(cancellationToken);

            // modify members of channel
            foreach (var channel in lstChannels)
            {
                if (channel.isPublic)
                {
                    channel.Members = users;
                }
                else
                {
                    channel.Members = channel.Members.Where(member =>
                        !deletedUsers.Any(deleteUser => deleteUser.UserOid.Equals(member.UserOid))).ToList();
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
