using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.TaskItems.Queries.GetTaskItemsList
{
    public class GetTaskItemsQueryHandler : IRequestHandler<GetTaskItemsQuery, IEnumerable<TaskItemModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly int pageSize = 10;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Tasks";

        public GetTaskItemsQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemModel>> Handle(GetTaskItemsQuery request, CancellationToken cancellationToken)
        {
            List<TaskItem> tasks;
            if (string.IsNullOrEmpty(request.keyword))
            {
                if (request.pageIndex != null)
                {
                    tasks = await _context.TaskItems.Where(t => t.TeamId == request.TeamId)
                            .Skip(((int)request.pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                }
                else
                {
                    tasks = await _context.TaskItems.Where(t => t.TeamId == request.TeamId).ToListAsync();
                }
            }
            else
            {
                var queryStr = "SELECT * FROM c " +
                                $"WHERE c.TeamId = '{request.TeamId}' AND (CONTAINS(UPPER(c.Name), '{request.keyword.ToUpper()}') " +
                                $" OR CONTAINS(UPPER(c.Assignee.DisplayName), '{request.keyword.ToUpper()}') " +
                                $" OR EXISTS(SELECT VALUE tag FROM tag IN c.Tags WHERE CONTAINS(UPPER(tag.TagName), '{request.keyword.ToUpper()}')))";
                tasks = await GetTasksAsync(queryStr);
            }

            Team team = await _context.Teams.FindAsync(request.TeamId);
            UserModel leader = _mapper.Map<User, UserModel>(team?.Users?.Where(u => u.TeamRole == "Leader").FirstOrDefault());
            List<TaskItemModel> taskItemModel = _mapper.Map<List<TaskItem>, List<TaskItemModel>>(tasks);
            foreach (TaskItemModel t in taskItemModel)
            {
                t.Leader = leader;
            }
            return taskItemModel;
        }

        private async Task<List<TaskItem>> GetTasksAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<TaskItem>(new QueryDefinition(queryStr));
            List<TaskItem> results = new List<TaskItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
