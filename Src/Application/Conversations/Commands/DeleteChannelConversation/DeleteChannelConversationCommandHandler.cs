using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.DeleteChannelConversation
{
    public class DeleteChannelConversationCommandHandler : IRequestHandler<DeleteChannelConversationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteChannelConversationCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteChannelConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _context.Conversations.FindAsync(new Guid(request.ConversationId));

            if (conversation == null || string.IsNullOrEmpty(conversation.TeamId))
            {
                throw new NotFoundException(nameof(Conversation), request.ConversationId);
            }
            var team = await _context.Teams.FindAsync(new Guid(conversation.TeamId));
            if (team == null)
            {
                throw new NotFoundException(nameof(Team), conversation.TeamId);
            }
            if (!conversation.CreatedBy.Equals(_currentUserService.UserId)
                && !_currentUserService.UserId.Equals(team.Users.First(u => u.TeamRole == "Leader").UserOid))
            {
                throw new NotOwnedException(nameof(Conversation), request.ConversationId);
            }
            var messageChats = await _context.MessageChats.Where(m => m.ConversationId == conversation.Id).ToListAsync();

            _context.MessageChats.RemoveRange(messageChats);
            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
