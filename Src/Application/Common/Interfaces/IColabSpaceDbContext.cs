using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ColabSpace.Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace ColabSpace.Application.Common.Interfaces
{
    public interface IColabSpaceDbContext
    {
        DbSet<Team> Teams { get; set; }

        DbSet<TaskItem> TaskItems { get; set; }

        DbSet<HelpItem> HelpItems { get; set; }

        DbSet<Conversation> Conversations { get; set; }
        
        DbSet<MessageChat> MessageChats { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<Planner> Planners { get; set; }

        CosmosClient GetCosmosClient { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
