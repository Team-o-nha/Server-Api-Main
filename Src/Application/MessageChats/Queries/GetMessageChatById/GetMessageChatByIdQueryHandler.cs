using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Queries.GetMessageChatById
{
    public class GetMessageChatByIdQueryHandler : IRequestHandler<GetMessageChatByIdQuery, MessageChatModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetMessageChatByIdQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MessageChatModel> Handle(GetMessageChatByIdQuery request, CancellationToken cancellationToken)
        {
            var messageChat = await _context.MessageChats.FindAsync(request.MessageChatId);

            if (messageChat == null)
            {
                throw new NotFoundException(nameof(MessageChat), request.MessageChatId);
            }

            return _mapper.Map<MessageChat, MessageChatModel>(messageChat);
        }
    }
}
