using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToContainer("Conversations");
            builder.OwnsMany<User>(p => p.Members).Ignore(u => u.Email).Ignore(u => u.GivenName)
                .Ignore(u => u.Surname).Ignore(u => u.TeamRole).Ignore(u => u.UserPrincipalName);
            builder.Property(i => i.isPublic).HasDefaultValue(false);
            builder.HasNoDiscriminator();
        }
    }
}
