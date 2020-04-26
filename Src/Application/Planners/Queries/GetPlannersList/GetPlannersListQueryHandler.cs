using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Planners.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Planners.Queries.GetPlannersList
{
    public class GetPlannersListQueryHandler : IRequestHandler<GetPlannersListQuery, IEnumerable<PlannerModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly int pageSize = 20;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Planners";

        public GetPlannersListQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlannerModel>> Handle(GetPlannersListQuery request, CancellationToken cancellationToken)
        {
            List<Planner> planners = null;
            if (string.IsNullOrEmpty(request.Keyword))
            {
                if (request.PageIndex != null)
                {
                    planners = await _context.Planners.Where(x => x.TeamId == request.TeamId)
                        .OrderByDescending(planner => planner.Created)
                        .Skip(((int)request.PageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                }
                else
                {
                    planners = await _context.Planners.Where(t => t.TeamId == request.TeamId).ToListAsync();
                }
            }
            else
            {
                var queryStr = "SELECT c.Id, c.Title FROM c " +
                                $"WHERE c.TeamId = '{request.TeamId}' AND (CONTAINS(UPPER(c.Title), '{request.Keyword.ToUpper()}') " +
                                $"OR CONTAINS(UPPER(c.Purpose), '{request.Keyword.ToUpper()}') " +
                                $"OR EXISTS(SELECT VALUE tag FROM tag IN c.Tags WHERE CONTAINS(UPPER(tag.TagName), '{request.Keyword.ToUpper()}')))";
                planners = await GetPlannersAsync(queryStr);
            }

            return _mapper.Map<List<PlannerModel>>(planners);
        }

        private async Task<List<Planner>> GetPlannersAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<Planner>(new QueryDefinition(queryStr));
            List<Planner> results = new List<Planner>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
