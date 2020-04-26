using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsByName
{
    public class GetConversationsByNameQueryHandler : IRequestHandler<GetConversationsByNameQuery, IEnumerable<ConversationModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Conversations";

        public GetConversationsByNameQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConversationModel>> Handle(GetConversationsByNameQuery request, CancellationToken cancellationToken)
        {
            var queryStr = $"SELECT c.id, c.Name, c.teamId, c.Type, " +
                    $" c.isHidden, c.isPublic, c.Members, " +
                    $" c.Created ,c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                    $" FROM c JOIN zc IN c.Members" +
                    $" WHERE zc.UserOid = '{request.LoginUserId.ToString()}' AND c.Type <> 'channel' AND CONTAINS(UPPER(c.Name), '{request.ConversationName?.ToUpper()}')";

            var conversations = await GetConversationsAsync(queryStr);
            List<Conversation> result = new List<Conversation>();
            foreach (var conversation in conversations)
            {
                if (_context.MessageChats.Where(m => m.ConversationId == conversation.Id).ToList().Count != 0)
                {
                    result.Add(conversation);
                }
            }
            return _mapper.Map<List<Conversation>, List<ConversationModel>>(result);
        }

        private async Task<List<Conversation>> GetConversationsAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<Conversation>(new QueryDefinition(queryStr));
            List<Conversation> results = new List<Conversation>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
