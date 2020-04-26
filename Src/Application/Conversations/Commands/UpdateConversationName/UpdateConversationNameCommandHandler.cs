using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.UpdateConversationName
{
    public class UpdateConversationNameCommandHandler : IRequestHandler<UpdateConversationNameCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public UpdateConversationNameCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateConversationNameCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _context.Conversations.FindAsync(request.Id);

            if (conversation == null)
            {
                throw new NotFoundException(nameof(Conversation), request.Id);
            }

            conversation.Name = request.Name;

            if (request.Members?.Count > 0)
            {
                conversation.Members = _mapper.Map<List<User>>(request.Members);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
