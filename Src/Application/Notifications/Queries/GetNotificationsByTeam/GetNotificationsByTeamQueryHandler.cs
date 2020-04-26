using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Notifications.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Notifications.Queries.GetNotificationsByTeam
{
    public class GetNotificationsByTeamQueryHandler : IRequestHandler<GetNotificationsByTeamQuery, IEnumerable<NotificationByTeamModel>>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "Teams";

        public GetNotificationsByTeamQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationByTeamModel>> Handle(GetNotificationsByTeamQuery request, CancellationToken cancellationToken)
        {
            // lay danh sach team cua login user
            //var teams = _context.Teams.Where(team => team.Id == request.UserId);
            var queryStr = $"SELECT c.id, c.Name, c.Description, c.Users " +
                                $"FROM c JOIN zc IN c.Users " +
                                $"WHERE zc.UserOid = '{request.UserId}'";
            var teams = await GetTeamsAsync(queryStr);

            var notificationModels = new List<NotificationByTeamModel>();

            foreach (var team in teams)
            {
                var notificationsByTeam = await _context.Notifications
                    // lay notification cua login user
                    .Where(notification => notification.ToUser.UserOid == request.UserId.ToString()
                        // chi lay notification co lien quan den Team
                        && notification.TeamId == team.Id.ToString())
                    .OrderByDescending(notification => notification.Created)
                    .Take(20)
                    .ToListAsync(cancellationToken);

                notificationModels.Add(new NotificationByTeamModel{
                    TeamId = team.Id,
                    TeamName = team.Name,
                    Notifications = _mapper.Map<List<NotificationModel>>(notificationsByTeam)
                });
            }

            return notificationModels;
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
