using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    public class PlannerConfiguration : IEntityTypeConfiguration<Planner>
    {
        ValueConverter converter = new ValueConverter<ICollection<Guid>, string>(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => new Guid(val)).ToList());

        public void Configure(EntityTypeBuilder<Planner> builder)
        {
            builder.ToContainer("Planners");
            builder.OwnsMany(p => p.Tags);
            builder.OwnsMany(p => p.Milestones, a =>
            {
                a.Property(p => p.TaskIds).HasConversion(converter);
            });
            builder.HasNoDiscriminator();
        }
    }
}
