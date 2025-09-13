using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public interface IAccountValidator : IDefaultValidator<Account, AccountId>
{
}
