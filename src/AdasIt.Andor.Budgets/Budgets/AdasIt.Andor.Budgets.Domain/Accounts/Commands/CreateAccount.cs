using AdasIt.Andor.Domain.Commands;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Commands;

public record CreateAccount : Command
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public Guid CurrencyId { get; init; }
}
