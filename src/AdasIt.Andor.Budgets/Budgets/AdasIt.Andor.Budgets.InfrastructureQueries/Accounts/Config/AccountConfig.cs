using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.Accounts.Config;

public record AccountConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(nameof(Account), SchemasNames.Budget);
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetAccountIdConverter());
        
        builder.Property(k => k.Name)
            .HasConversion(Converters.GetNameConverter())
            .HasMaxLength(Name.MaxLength);
        
        builder.Property(k => k.Description)
            .HasConversion(Converters.GetDescriptionConverter())
            .HasMaxLength(Description.MaxLength);
        
        builder.Property(k => k.Status)
            .HasConversion(GetAccountStatusConverter());

        builder.Ignore(x => x.Events);
        
        builder.Navigation(x => x.Categories).AutoInclude();
        builder.Navigation(x => x.SubCategories).AutoInclude();
        builder.Navigation(x => x.PaymentMethods).AutoInclude();
        builder.Navigation(x => x.Invites).AutoInclude();
    }

    private static ValueConverter<AccountId, Guid> GetAccountIdConverter()
        => new(id => id!.Value, value => AccountId.Load(value));
    
    private static ValueConverter<AccountStatus, int> GetAccountStatusConverter()
        => new(v => v.Key, v => AccountStatus.GetByKey<AccountStatus>(v));
}