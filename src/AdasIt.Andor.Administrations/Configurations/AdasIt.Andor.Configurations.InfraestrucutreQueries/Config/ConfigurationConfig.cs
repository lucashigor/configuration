using AdasIt.Andor.Configurations.DomainQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdasIt.Andor.Configurations.InfrastructureQueries.Config;

public class ConfigurationProjectionConfig : IEntityTypeConfiguration<ConfigurationOutput>
{
    public void Configure(EntityTypeBuilder<ConfigurationOutput> entity)
    {
        entity.ToTable("ConfigurationBasicProjection", "Administration");
        entity.HasKey(k => k.Id);
        entity.Property(k => k.Name).HasMaxLength(70);
        entity.Property(k => k.Value).HasMaxLength(2500);
        entity.Property(k => k.Description).HasMaxLength(1000);
        entity.Property(k => k.CreatedBy).HasMaxLength(70);

        entity.Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone");
        entity.Property(x => x.StartDate)
            .HasColumnType("timestamp with time zone");
        entity.Property(x => x.ExpireDate)
            .HasColumnType("timestamp with time zone");

        entity.Ignore(k => k.State);
    }
}
