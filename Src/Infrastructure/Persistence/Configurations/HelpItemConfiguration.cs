using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    public class HelpItemConfiguration : IEntityTypeConfiguration<HelpItem>
    {
        public void Configure(EntityTypeBuilder<HelpItem> builder)
        {
            builder.ToContainer("HelpItems");

            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(20);
            builder.Property(p => p.Description)
                .HasMaxLength(500);
            builder.OwnsOne<AttachFile>(p => p.Content);

            //builder.OwnsOne<User>(p => p.CreatedBy).Ignore(u => u.Email).Ignore(u => u.GivenName)
            //    .Ignore(u => u.Surname).Ignore(u => u.TeamRole).Ignore(u => u.UserPrincipalName);
            //builder.OwnsMany<AttachFile>(p => p.AttachFiles);


            builder.HasNoDiscriminator();
        }
    }
}
