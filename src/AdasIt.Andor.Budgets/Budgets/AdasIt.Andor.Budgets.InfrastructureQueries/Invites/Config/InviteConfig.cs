using AdasIt.Andor.Budgets.Domain.Invites;
using AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;
using AdasIt.Andor.Budgets.InfrastructureQueries.Users.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.Invites.Config;

public record InviteConfig : IEntityTypeConfiguration<Invite>
{
    public void Configure(EntityTypeBuilder<Invite> builder)
    {
        builder.ToTable(nameof(Invite), SchemasNames.Budget);
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(GetInviteIdConverter());
        builder.Property(k => k.GuestId).HasConversion(UserConfig.GetUserIdConverter());
        builder.Property(k => k.InvitingId).HasConversion(UserConfig.GetUserIdConverter());
        builder.Property(k => k.Email)
            .HasMaxLength(70);

        builder.Property(k => k.Status).HasConversion(
            state => state.Key,
            value => InviteStatus.GetByKey<InviteStatus>(value));
    }

    static ValueConverter<InviteId, Guid> GetInviteIdConverter()
       => new(id => id!.Value, value => InviteId.Load(value));
}