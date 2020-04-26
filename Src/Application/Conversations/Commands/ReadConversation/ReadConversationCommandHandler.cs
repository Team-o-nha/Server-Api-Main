using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Commands.HideConversation;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.ReadConversation
{
    public class ReadConversationCommandHandler : IRequestHandler<ReadConversationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public ReadConversationCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ReadConversationCommand request, CancellationToken cancellationToken)
        {
            Conversation conversation = await _context.Conversations.FindAsync(Guid.Parse(request.ConversationId));

            if (conversation == null)
            {
                throw new NotFoundException(nameof(Conversation), request.ConversationId);
            }

            foreach (User user in conversation.Members)
            {
                if (user.UserOid == _currentUserService.UserId)
                {
                    user.LastSeenTime = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}
