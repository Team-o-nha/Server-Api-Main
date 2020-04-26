using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Teams.Queries.GetTeamsList
{
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IEnumerable<TeamModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Teams";

        public GetTeamsQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamModel>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await GetTeamByQuery(request.UserId, request.TeamName, cancellationToken);
            return _mapper.Map<List<TeamModel>>(teams);
        }

        private async Task<List<Team>> GetTeamByQuery(string userId, string teamName, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var queryStr = $"SELECT c.id, c.Name, c.Description, c.Users " +
                                $"FROM c JOIN zc IN c.Users " +
                                $"WHERE zc.UserOid = '{userId}'";
                return await GetTeamsAsync(queryStr);
            }
            else if (!String.IsNullOrEmpty(teamName))
            {
                var queryStr = $"SELECT * FROM c where CONTAINS(UPPER(c.Name), '{teamName.ToUpper()}')";
                return await GetTeamsAsync(queryStr);
            } else
            {
                return await _context.Teams.ToListAsync(cancellationToken);
            }
        }

        private async Task<List<Team>> GetTeamsAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<Team>(new QueryDefinition(queryStr));
            List<Team> results = new List<Team>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
