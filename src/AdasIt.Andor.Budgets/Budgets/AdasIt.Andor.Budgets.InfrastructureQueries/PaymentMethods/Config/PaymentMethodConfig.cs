using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.InfrastructureQueries.FinancialMovements.Config;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.PaymentMethods.Config;

public record PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable(nameof(PaymentMethod), SchemasNames.Budget);
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetPaymentMethodIdConverter());
        
        builder.Property(k => k.Name)
            .HasConversion(Converters.GetNameConverter())
            .HasMaxLength(Name.MaxLength);
        
        builder.Property(k => k.Description)
            .HasConversion(Converters.GetDescriptionConverter())
            .HasMaxLength(Description.MaxLength);

        builder.Property(k => k.Type)
            .HasConversion(FinancialMovementConfig.GetMovementTypeConverter());
    }

    internal static ValueConverter<PaymentMethodId, Guid> GetPaymentMethodIdConverter()
        => new(id => id!.Value, value => PaymentMethodId.Load(value));
}