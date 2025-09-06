using AdasIt.Andor.Domain.Commands;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Commands;

public abstract record FinancialMovement : Command
{
    public DateTime Date { get; init; }
    public string Description { get; init; } = "";
    public int Type { get; init; }
    public int Status { get; init; }
    public Guid SubCategoryId { get; init; }
    public Guid PaymentMethodId { get; init; }
    public decimal Value { get; init; }

    public record Deposit : FinancialMovement
    {
    }

    public record Withdrawal : FinancialMovement
    {
    }
}
