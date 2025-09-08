using AdasIt.Andor.Domain.Commands;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Commands;

public record AddFinancialMovement(decimal Amount, string? Name, DateTime? date, Guid PaymentMethodId, 
    Guid SubCategoryId) : Command;