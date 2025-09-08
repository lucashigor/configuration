using System.Net.Mail;
using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.Users.Config;


public record UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable(nameof(User), SchemasNames.Budget);
        entity.HasKey(k => k.Id);
        entity.Property(k => k.Id).HasConversion(GetUserIdConverter());
        entity.Property(k => k.Email)
            .HasMaxLength(70)
            .HasConversion(
                email => email!.Address,
                value => new MailAddress(value));

        entity.Property(k => k.PreferredCurrencyId)
            .HasConversion(
                id => id!.Value,
                value => CurrencyId.Load(value)
            );

        entity.Property(k => k.PreferredLanguageId)
            .HasConversion(
                id => id!.Value,
                value => LanguageId.Load(value)
            );
    }

    internal static ValueConverter<UserId, Guid> GetUserIdConverter()
        => new(id => id!.Value, value => UserId.Load(value));
}