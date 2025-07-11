using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdasIt.Andor.Configurations.Infrastructure.Config;

internal class ConfigurationConfig : IEntityTypeConfiguration<ConfigurationDto>
{
    public void Configure(EntityTypeBuilder<ConfigurationDto> entity)
    {
        entity.ToTable(nameof(Configuration), SchemasNames.Administration);

        entity.HasKey(k => k.Id);
        entity.Property(k => k.Name).HasMaxLength(70);
        entity.Property(k => k.Value).HasMaxLength(2500);
        entity.Property(k => k.Description).HasMaxLength(1000);
        entity.Property(k => k.CreatedBy).HasMaxLength(70);
    }
}