using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.FinancialMovements;

public interface IFinancialMovementValidator : IDefaultValidator<FinancialMovement, FinancialMovementId>
{
}
