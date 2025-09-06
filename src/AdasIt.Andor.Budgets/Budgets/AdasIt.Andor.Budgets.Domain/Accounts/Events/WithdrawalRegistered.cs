using AdasIt.Andor.Domain.Events;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Events;

public record WithdrawalRegistered : DomainEvent
{
    public DateTime Date { get; init; }
    public string Description { get; init; } = "";
    public int Type { get; init; }
    public int Status { get; init; }
    public Guid SubCategoryId { get; init; }
    public Guid PaymentMethodId { get; init; }
    public decimal Value { get; init; }
}
