using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.FinancialMovements;
using AdasIt.Andor.Budgets.InfrastructureQueries.PaymentMethods.Config;
using AdasIt.Andor.Budgets.InfrastructureQueries.SubCategories.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.FinancialMovements.Config;

public record FinancialMovementConfig : IEntityTypeConfiguration<FinancialMovement>
{
    public void Configure(EntityTypeBuilder<FinancialMovement> builder)
    {
        builder.ToTable(nameof(FinancialMovement), SchemasNames.Budget);
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetFinancialMovementIdConverter());
        builder.Property(k => k.PaymentMethodId).HasConversion(PaymentMethodConfig.GetPaymentMethodIdConverter());
        builder.Property(k => k.SubCategoryId).HasConversion(SubCategoryConfig.GetSubCategoryIdConverter());
        builder.Property(k => k.Type).HasConversion(GetMovementTypeConverter());
        builder.Property(k => k.Status).HasConversion(GetMovementStatusonverter());
        
    }

    private static ValueConverter<FinancialMovementId, Guid> GetFinancialMovementIdConverter()
        => new(id => id!.Value, value => FinancialMovementId.Load(value));
    
    internal static ValueConverter<MovementType, int> GetMovementTypeConverter()
        => new(v => v.Key, v => MovementType.GetByKey<MovementType>(v));
    
    private static ValueConverter<MovementStatus, int> GetMovementStatusonverter()
        => new(v => v.Key, v => MovementStatus.GetByKey<MovementStatus>(v));
}