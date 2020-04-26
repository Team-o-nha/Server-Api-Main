using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public CreateConversationCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ConversationModel> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var validConversationType = new List<string> { "pair", "group", "channel" };
            // check conversation type is valid
            if (!validConversationType.Contains(request.Type))
            {
                throw new NotTypeException(request.Type, validConversationType);
            }

            // copy prop from request
            var conversation = new Conversation
            {
                Name = request.Name,
                Type = request.Type,
                Members = _mapper.Map<List<User>>(request.Members),
                isPublic = request.IsPublic,
                TeamId = request.TeamId
            };

            // register new conversation
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ConversationModel>(conversation);
        }
    }
}
