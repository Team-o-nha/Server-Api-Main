using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Queries.GetMessageChatList
{
    public class GetMessageChatsQueryHandler : IRequestHandler<GetMessageChatsQuery, IEnumerable<MessageChatModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly int pageSize = 30;

        public GetMessageChatsQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageChatModel>> Handle(GetMessageChatsQuery request, CancellationToken cancellationToken)
        {
            List<MessageChat> lstMessChat = null;

            if (request.PageIndex != null)
            {
                lstMessChat = await _context.MessageChats
                    .Where(messChat => messChat.ConversationId == request.ConversationId)
                    .OrderByDescending(messChat => messChat.Created)
                    .Skip(((int)request.PageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
            else if (request.ConversationId != Guid.Empty)
            {
                lstMessChat = await _context.MessageChats
                    .Where(messChat => messChat.ConversationId == request.ConversationId)
                    .OrderBy(messChat => messChat.Created)
                    .ToListAsync();
            }
            else
            {
                lstMessChat = await _context.MessageChats
                                .Where(messChat =>
                                (messChat.RelatedMessagesId == request.RelatedMessagesId
                                && messChat.RelatedTaskId == request.RelatedTaskId)
                                || messChat.Id == request.RelatedMessagesId)
                                .OrderBy(messChat => messChat.Created)
                                .ToListAsync();
            }

            lstMessChat.ForEach(p =>
            {
                p.RelatedMessages = _context.MessageChats.Find(p.RelatedMessagesId);
            });

            return _mapper.Map<List<MessageChatModel>>(lstMessChat.OrderBy(x => x.Created));
        }
    }
}
