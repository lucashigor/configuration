using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Budgets.Domain.Users.Events;

public record UserCreatedDomainEvent : DomainEvent
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid PreferredCurrencyId { get; set; }
    public Guid PreferredLanguageId { get; set; }

    public static UserCreatedDomainEvent FromAggregateRoot(User entity)
        => new UserCreatedDomainEvent() with
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email.Address,
            PreferredCurrencyId = (Guid)entity.PreferredCurrencyId,
            PreferredLanguageId = (Guid)entity.PreferredLanguageId
        };
}
