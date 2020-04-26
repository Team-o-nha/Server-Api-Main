using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToContainer("Notifications");
            builder.OwnsOne<User>(p => p.ToUser).Ignore(u => u.Email).Ignore(u => u.GivenName)
                .Ignore(u => u.Surname).Ignore(u => u.TeamRole).Ignore(u => u.UserPrincipalName).Ignore(u => u.LastSeenTime).Ignore(u => u.isHidden);
            builder.Property(i => i.isRead).HasDefaultValue(false);
            builder.HasNoDiscriminator();
        }
    }
}
