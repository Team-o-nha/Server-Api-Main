using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Queries.GetLastContentOfConversation
{
    public class GetLastContentOfConversationQueryHandler : IRequestHandler<GetLastContentOfConversationQuery, IEnumerable<ConversationLastContentModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Conversations";

        public GetLastContentOfConversationQueryHandler(IColabSpaceDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ConversationLastContentModel>> Handle(GetLastContentOfConversationQuery request, CancellationToken cancellationToken)
        {
            var count = 0; //UnreadCounter
            var result = new List<ConversationLastContentModel>();
            var listConversations = new List<Conversation>();

            if ((!String.IsNullOrEmpty(request.UserId.ToString()) && String.IsNullOrEmpty(request.TeamId.ToString())) ||
                ((!String.IsNullOrEmpty(request.UserId.ToString()) && Guid.Empty == request.TeamId)))
            {
                var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
                                $",c.isHidden, c.isPublic, c.Members, c.Created " +
                                $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                                $"FROM c JOIN zc IN c.Members " +
                                $"WHERE zc.UserOid = '{request.UserId.ToString()}' AND zc.isHidden = false AND c.Type <> 'channel'";
                listConversations = await GetConversationsAsync(queryStr);
            }
            else if (!String.IsNullOrEmpty(request.UserId.ToString()) && !String.IsNullOrEmpty(request.TeamId.ToString()))
            {
                var queryStr = $"SELECT c.id, c.Name, c.TeamId, c.Type " +
                $",c.isHidden, c.isPublic, c.Members, c.Created " +
                $",c.CreatedBy, c.LastModified, c.LastModifiedBy " +
                $"FROM c JOIN zc IN c.Members " +
                $"WHERE zc.UserOid = '{request.UserId.ToString()}' AND zc.isHidden = false AND c.TeamId = '{request.TeamId.ToString()}' AND c.Type = 'channel'";
                listConversations = await GetConversationsAsync(queryStr);
            }

            for (int i = 0; i < listConversations.Count; i++)
            {
                var listMessChat = await _context.MessageChats
                    .Where(messChat => messChat.ConversationId == listConversations[i].Id)
                    .OrderBy(messChat => messChat.Created).ToListAsync();
                var lastMessChat = listMessChat.LastOrDefault();
                var lastContent = _mapper.Map<MessageChat, MessageChatModel>(lastMessChat);

                //var listMessChatConverted = _mapper.Map<List<MessageChat>, List<MessageChatModel>>(listMessChat);

                // conversation already had message
                if (lastMessChat != null)
                {
                    // get members in conversation
                    foreach (var member in listConversations[i].Members)
                    {
                        if (lastMessChat.CreatedBy != _currentUserService.UserId)
                        {
                            if (member.UserOid == _currentUserService.UserId)
                            {
                                // new message check
                                if (DateTime.Compare(member.LastSeenTime, lastMessChat.Created) < 0)
                                {
                                    count = 1;
                                }
                                else if (DateTime.Compare(member.LastSeenTime, lastMessChat.Created) >= 0)
                                {
                                    count = 0;
                                }
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                    }

                    var conv = _mapper.Map<Conversation, ConversationModel>(listConversations[i]);
                    result.Add(new ConversationLastContentModel
                    {
                        Conversation = conv,
                        LastMessageChatContent = lastContent,
                        UnreadCounter = count
                    });
                }
                else if (lastMessChat == null && listConversations[i].Type.ToLower() != "pair" && listConversations[i].Type.ToLower() != "group")
                {
                    var conv = _mapper.Map<Conversation, ConversationModel>(listConversations[i]);
                    result.Add(new ConversationLastContentModel
                    {
                        Conversation = conv,
                        LastMessageChatContent = new MessageChatModel() { Created = DateTime.MinValue },
                        UnreadCounter = count
                    });
                }
            }

            return result.OrderBy(m => m.LastMessageChatContent.Created);
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