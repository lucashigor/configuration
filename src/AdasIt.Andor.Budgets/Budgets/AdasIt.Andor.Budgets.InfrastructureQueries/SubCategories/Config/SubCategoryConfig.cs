using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Budgets.InfrastructureQueries.PaymentMethods.Config;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.SubCategories.Config;

public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable(nameof(SubCategory), SchemasNames.Budget);
        
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetSubCategoryIdConverter());
        
        builder.Property(k => k.Name)
            .HasConversion(Converters.GetNameConverter())
            .HasMaxLength(Name.MaxLength);
        
        builder.Property(k => k.Description)
            .HasConversion(Converters.GetDescriptionConverter())
            .HasMaxLength(Description.MaxLength);
        
        builder.Property(k => k.DefaultPaymentMethodId).HasConversion(
            PaymentMethodConfig.GetPaymentMethodIdConverter());

        builder.HasOne(k => k.Category).WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.CategoryId);

        builder.Navigation(x => x.Category).AutoInclude();
        builder.Navigation(x => x.DefaultPaymentMethod).AutoInclude();
    }

    internal static ValueConverter<SubCategoryId, Guid> GetSubCategoryIdConverter()
        => new(id => id!.Value, value => SubCategoryId.Load(value));
}