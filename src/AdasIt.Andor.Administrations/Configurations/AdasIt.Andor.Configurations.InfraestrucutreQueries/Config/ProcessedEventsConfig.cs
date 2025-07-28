using AdasIt.Andor.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdasIt.Andor.Configurations.InfrastructureQueries.Config;

public class ProcessedEventsConfig : IEntityTypeConfiguration<ProcessedEvents>
{
    public void Configure(EntityTypeBuilder<ProcessedEvents> entity)
    {
        entity.ToTable(nameof(ProcessedEvents), "Administration");
        entity.HasKey(k => new { k.AggregatorId, k.EventId, k.ProjectionName });
        entity.Property(k => k.ProjectionName).HasMaxLength(250);
    }
}
