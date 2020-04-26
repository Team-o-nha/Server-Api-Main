using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Common;
using ColabSpace.Domain.Entities;
using ColabSpace.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Infrastructure.Persistence
{
    public class ColabSpaceDbContext : DbContext, IColabSpaceDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ColabSpaceDbContext(DbContextOptions<ColabSpaceDbContext> options)
            : base(options)
        {
            base.Database.EnsureCreated();
        }

        public ColabSpaceDbContext(DbContextOptions<ColabSpaceDbContext> options, 
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            base.Database.EnsureCreated();
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        protected ColabSpaceDbContext()
        {
            base.Database.EnsureCreated();
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<HelpItem> HelpItems { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<MessageChat> MessageChats { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Planner> Planners { get; set; }

        public CosmosClient GetCosmosClient => base.Database.GetCosmosClient();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //If this is not set then the default name will be the name of the DbContext => ColabSpaceDbContext
            modelBuilder.HasDefaultContainer("ColabSpaceContainer");

            //Manually setting container names for DbSets
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColabSpaceDbContext).Assembly);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? entry.Entity.CreatedBy;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId ?? entry.Entity.CreatedBy;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
