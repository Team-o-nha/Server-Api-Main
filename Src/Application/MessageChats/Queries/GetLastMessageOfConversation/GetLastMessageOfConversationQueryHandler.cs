using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Queries.GetLastMessageOfConversation
{
    public class GetLastMessageOfConversationQueryHandler : IRequestHandler<GetLastMessageOfConversationQuery, MessageChatModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetLastMessageOfConversationQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MessageChatModel> Handle(GetLastMessageOfConversationQuery request, CancellationToken cancellationToken)
        {
            var recentMessageChat = await _context.MessageChats
                // lay danh sach message trong conversation
                .Where(msg => msg.ConversationId == request.ConversationId)
                // sap xep giam dan theo ngay tao message
                .OrderByDescending(msg => msg.Created)
                // lay message gan nhat
                .FirstOrDefaultAsync();

            return _mapper.Map<MessageChatModel>(recentMessageChat);
        }
    }
}
