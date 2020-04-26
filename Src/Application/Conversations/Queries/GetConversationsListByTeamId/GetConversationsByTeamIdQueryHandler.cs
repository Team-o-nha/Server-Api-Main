using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsListByTeamId
{
    public class GetConversationsByTeamIdQueryHandler : IRequestHandler<GetConversationsByTeamIdQuery, IEnumerable<ConversationModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetConversationsByTeamIdQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConversationModel>> Handle(GetConversationsByTeamIdQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _context.Conversations.Where(c => c.TeamId == request.TeamId.ToString()).ToListAsync();
            return _mapper.Map<List<Conversation>, List<ConversationModel>>(conversations);
        }
    }
}
