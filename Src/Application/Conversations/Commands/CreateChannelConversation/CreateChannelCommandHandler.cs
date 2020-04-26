using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Commands.CreateChannelConversation
{
    public class CreateChannelConversationCommandHandler : IRequestHandler<CreateChannelConversationCommand, ConversationModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public CreateChannelConversationCommandHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ConversationModel> Handle(CreateChannelConversationCommand request, CancellationToken cancellationToken)
        {
            Team team = await _context.Teams.FindAsync(Guid.Parse(request.TeamId));

            if (team == null)
            {
                throw new NotFoundException(nameof(Team), request.TeamId);
            }

            var conversation = new Conversation();
            var allChannel = _context.Conversations
           .Where(conversation => conversation.TeamId == request.TeamId && conversation.Type == "channel").ToList();

            foreach (var temp in allChannel)
            {
                if (temp.Name == request.Name)
                {
                    throw new ArgumentException(String.Format("This name had exsisted: {0}", request.Name));
                }
            }

            if (request.IsPublic == true)
            {
                // copy prop from request
                conversation = new Conversation
                {
                    Name = request.Name,
                    ChannelDescription = request.ChannelDescription,
                    Type = "channel",
                    Members = team.Users,
                    isPublic = request.IsPublic,
                    TeamId = request.TeamId
                };
            }
            else
            {
                conversation = new Conversation
                {
                    Name = request.Name,
                    ChannelDescription = request.ChannelDescription,
                    Type = "channel",
                    Members = _mapper.Map<List<User>>(request.Members),
                    isPublic = request.IsPublic,
                    TeamId = request.TeamId
                };
            }

            // register new conversation
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ConversationModel>(conversation);
        }
    }
}
