using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.HideConversation
{
    public class HideConversationCommandHandler : IRequestHandler<HideConversationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public HideConversationCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(HideConversationCommand request, CancellationToken cancellationToken)
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
                    user.isHidden = true;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
