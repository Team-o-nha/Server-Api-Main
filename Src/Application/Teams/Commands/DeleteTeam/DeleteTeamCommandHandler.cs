using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Teams.Commands.DeleteTeam
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTeamCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
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

            var taskItems = await _context.TaskItems.Where(t => t.TeamId == team.Id).ToListAsync();

            // find all channel when delete team
            var channels = await _context.Conversations.Where(t => t.TeamId == team.Id.ToString()).ToListAsync();

            // find all message chat
            var messageChats = new List<MessageChat>();
            foreach (var channel in channels)
            {
                // find MessageChat async (tim cac MessageChat cung mot luc)
                messageChats.AddRange(await _context.MessageChats.Where(t => t.ConversationId == channel.Id).ToListAsync());
            }

            _context.MessageChats.RemoveRange(messageChats);

            _context.TaskItems.RemoveRange(taskItems);

            _context.Conversations.RemoveRange(channels);

            _context.Teams.Remove(team);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
