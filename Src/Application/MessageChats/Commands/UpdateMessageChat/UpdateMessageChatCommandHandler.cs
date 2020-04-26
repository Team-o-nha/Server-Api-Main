using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;

namespace ColabSpace.Application.MessageChats.Commands.UpdateMessageChat
{
    public class UpdateMessageChatCommandHandler : IRequestHandler<UpdateMessageChatCommand, string>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateMessageChatCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(UpdateMessageChatCommand request, CancellationToken cancellationToken)
        {
            var messageChat = await _context.MessageChats.FindAsync(request.MessageId);
            var result = "";
            if (messageChat == null)
            {
                throw new NotFoundException(nameof(MessageChat), request.MessageId);
            }

            if (messageChat.ReactionList == null)
            {
                messageChat.ReactionList = new List<Reaction>();
            }
            if (request.ReactionType != null)
            {
                var reaction = messageChat.ReactionList.FirstOrDefault(r => r.ReactorId == _currentUserService.UserId);
                if (reaction == null)
                {
                    messageChat.ReactionList.Add(new Reaction
                    {
                        ReactorId = _currentUserService.UserId,
                        ReactorName = _currentUserService.UserName,
                        ReactionType = request.ReactionType
                    });
                    result = "addReaction";
                }
                else
                {
                    messageChat.ReactionList.Remove(reaction);
                    result = "deleteReaction";
                    if (!reaction.ReactionType.Equals(request.ReactionType))
                    {
                        messageChat.ReactionList.Add(new Reaction
                        {
                            ReactorId = _currentUserService.UserId,
                            ReactorName = _currentUserService.UserName,
                            ReactionType = request.ReactionType
                        });
                        result = "updateReaction";
                    }
                }
            }
            if (request.IsPin.HasValue)
            {
                messageChat.IsPin = request.IsPin.Value;
                messageChat.PinnedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
