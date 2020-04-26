using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;

namespace ColabSpace.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
    {
        private readonly IColabSpaceDbContext _context;

        public CreateTeamCommandHandler(IColabSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            // create team
            var users = new List<User>();
            foreach (UserModel userModel in request.Users)
            {
                users.Add(new User
                {
                    UserOid = userModel.UserId.ToString(),
                    DisplayName = userModel.DisplayName,
                    Email = userModel.Email,
                    Surname = userModel.Surname,
                    GivenName = userModel.GivenName,
                    UserPrincipalName = userModel.UserPrincipalName,
                    TeamRole = userModel.TeamRole
                });                
            }
            var team = new Team()
            {
                Name = request.Name,
                Users = users,
                Description = request.Description,
            };

            _context.Teams.Add(team);

            // create general channel in team        
            var conversation = new Conversation
            {
                Name = "General",
                Type = "channel",
                Members = users,
                isPublic = true,
                TeamId = team.Id.ToString()
            };

            // register new conversation
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }
}
