using AutoMapper;
using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Queries.GetConversation
{
    public class GetConversationQueryHandler : IRequestHandler<GetConversationQuery, ConversationModel>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Conversations";

        public GetConversationQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ConversationModel> Handle(GetConversationQuery request, CancellationToken cancellationToken)
        {
            Conversation conversation;
            if (!string.IsNullOrEmpty(request.ConversationId))
            {
                conversation = await _context.Conversations.FindAsync(new Guid(request.ConversationId));
                if (conversation == null)
                {
                    throw new NotFoundException(nameof(Conversation), request.ConversationId);
                }
            }
            else
            {
                var listMember = request.MembersId.Select(id => "AND ARRAY_CONTAINS(c.Members, {'UserOid': '" + id + "'}, true)").ToList();
                var queryStr = $"SELECT c.id, c.Name, c.Type, c.Members, c.isHidden, c.TeamId, c.isPublic " +
                                $"FROM c " +
                                $"WHERE ARRAY_LENGTH(c.Members) = {listMember.Count} " +
                                $"{string.Join(' ', listMember)} " + 
                                $"AND c.Type != 'channel' ";
                conversation = await GetConversationAsync(queryStr);
            }
            if (conversation != null)
            {
                Conversation tempConversation = await _context.Conversations.FindAsync(conversation.Id);
                foreach (User user in tempConversation.Members)
                {
                    if (user.isHidden && user.UserOid == request.LoginUserId)
                    {
                        user.isHidden = false;
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
            }


            return _mapper.Map<Conversation, ConversationModel>(conversation);
        }

        private async Task<Conversation> GetConversationAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<Conversation>(new QueryDefinition(queryStr));
            Conversation result = null;
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                if (response.Resource.Any())
                {
                    result = response.First();
                }
            }

            return result;
        }
    }
}