using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToContainer("Teams");
            builder.OwnsMany<User>(p => p.Users).Ignore(p => p.isHidden).Ignore(p => p.LastSeenTime);
            builder.HasNoDiscriminator();
        }
    }
}