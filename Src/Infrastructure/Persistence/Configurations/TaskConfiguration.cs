using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToContainer("Tasks");
            builder.OwnsOne<User>(p => p.CreatedBy).Ignore(u => u.Email).Ignore(u => u.GivenName)
                .Ignore(u => u.Surname).Ignore(u => u.TeamRole).Ignore(u => u.UserPrincipalName).Ignore(p => p.isHidden).Ignore(p => p.LastSeenTime);
            builder.OwnsOne<User>(p => p.Assignee).Ignore(u => u.Email).Ignore(u => u.GivenName)
                .Ignore(u => u.Surname).Ignore(u => u.TeamRole).Ignore(u => u.UserPrincipalName).Ignore(p => p.isHidden).Ignore(p => p.LastSeenTime);
            builder.OwnsMany<AttachFile>(p => p.AttachFiles);
            builder.OwnsMany<History>(p => p.Histories);
            builder.OwnsMany<RelatedObject>(p => p.Relations);
            builder.Property(p => p.Name).IsRequired();
            builder.Ignore(p => p.Leader);
            builder.OwnsMany<Tag>(p => p.Tags);
            builder.HasNoDiscriminator();
        }
    }
}