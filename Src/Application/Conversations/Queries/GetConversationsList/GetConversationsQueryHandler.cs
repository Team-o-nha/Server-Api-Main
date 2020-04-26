using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Conversations.Queries.GetConversationsList
{
    public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, IEnumerable<ConversationModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Conversations";

        public GetConversationsQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConversationModel>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await GetConversationsByQuery(request.UserId.ToString(), request.ConversationName, request.TeamId.ToString(), cancellationToken);
            return _mapper.Map<List<Conversation>, List<ConversationModel>>(conversations);
        }

        private async Task<List<Conversation>> GetConversationsByQuery(string userId, string conversationName, string teamId, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(conversationName))
            {
                var queryStr = $"SELECT c.id, c.Name, c.teamId, c.Type, " +
                    $" c.isHidden, c.isPublic, c.Members, " +
                    $" c.Created ,c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                    $" FROM c JOIN zc IN c.Members" +
                    $" WHERE zc.UserOid = '{userId}' AND c.Type <> 'channel' AND CONTAINS(UPPER(c.Name), '{conversationName.ToUpper()}')";
                return await GetConversationsAsync(queryStr);
            }
            else if (Guid.Parse(teamId) != Guid.Empty)
            {
                var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
                                    $",c.isHidden, c.isPublic, c.Members, c.Created " +
                                    $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                                    $"FROM c JOIN zc IN c.Members " +
                                    $"WHERE zc.UserOid = '{userId}' AND c.TeamId = '{teamId}' AND c.Type = 'channel'";
                return await GetConversationsAsync(queryStr);
            }
            else
            {
                var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
                $",c.isHidden, c.isPublic, c.Members, c.Created " +
                $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                $"FROM c JOIN zc IN c.Members " +
                $"WHERE zc.UserOid = '{userId}' AND c.Type <> 'channel'";
                return await GetConversationsAsync(queryStr);
            }
        }
        //public async Task<IEnumerable<ConversationModel>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        //{
        //    List<Conversation> conversations;
        //    if (!String.IsNullOrEmpty(request.ConversationName))
        //    {
        //        var queryStr = $"SELECT c.id, c.Name, c.teamId, c.Type, " +
        //            $" c.isHidden, c.isPublic, c.Members, " +
        //            $" c.Created ,c.CreatedBy, c.LastModified, c.LastModifiedBy " +
        //            $" FROM c JOIN zc IN c.Members" +
        //            $" WHERE zc.UserOid = '{request.UserId.ToString()}' AND c.Type <> 'channel' AND CONTAINS(UPPER(c.Name), '{request.ConversationName.ToUpper()}')";
        //        conversations = await GetConversationsAsync(queryStr);
        //    }
        //    else if (request.TeamId != Guid.Empty)
        //    {
        //        var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
        //                            $",c.isHidden, c.isPublic, c.Members, c.Created " +
        //                            $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
        //                            $"FROM c JOIN zc IN c.Members " +
        //                            $"WHERE zc.UserOid = '{request.UserId.ToString()}' AND c.TeamId = '{request.TeamId.ToString()}' AND c.Type = 'channel'";
        //        conversations = await GetConversationsAsync(queryStr);
        //    }
        //    else
        //    {
        //        var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
        //        $",c.isHidden, c.isPublic, c.Members, c.Created " +
        //        $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
        //        $"FROM c JOIN zc IN c.Members " +
        //        $"WHERE zc.UserOid = '{request.UserId.ToString()}' AND c.Type <> 'channel'";
        //        conversations = await GetConversationsAsync(queryStr);
        //    }

        //    return _mapper.Map<List<Conversation>, List<ConversationModel>>(conversations);
        //}

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
