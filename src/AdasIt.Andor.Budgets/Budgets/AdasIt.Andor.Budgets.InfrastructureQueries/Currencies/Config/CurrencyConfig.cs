using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.Currencies.Config;

public record CurrencyConfig : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable(nameof(Currency), SchemasNames.Budget);
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetCurrencyIdConverter());
        
        builder.Property(k => k.Name)
            .HasConversion(Converters.GetNameConverter())
            .HasMaxLength(Name.MaxLength);
        
        builder.Property(e => e.Iso)
            .HasConversion(
                v => v.Value,
                v => Iso.Load(v))
            .HasMaxLength(Symbol.MaxLength);
        
        builder.Property(e => e.Symbol)
            .HasConversion(
                v => v.Value,
                v => Symbol.Load(v))
            .HasMaxLength(Symbol.MaxLength);
    }

    private static ValueConverter<CurrencyId, Guid> GetCurrencyIdConverter()
        => new(id => id!.Value, value => CurrencyId.Load(value));
}