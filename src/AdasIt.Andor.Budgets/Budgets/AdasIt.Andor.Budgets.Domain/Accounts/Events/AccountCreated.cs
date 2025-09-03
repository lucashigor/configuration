using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Events;

public record AccountCreated : DomainEvent
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public Guid CurrencyId { get; init; }
    public int StatusId { get; init; }

    public static AccountCreated FromConfiguration(Account account)
        => new AccountCreated() with
        {
            Id = account.Id,
            Name = account.Name,
            Description = account.Description,
            CurrencyId = account.Currency.Id,
            StatusId = account.Status.Key
        };
}
