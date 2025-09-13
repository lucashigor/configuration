using AdasIt.Andor.Budget.Application.Interfaces;
using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;

namespace AdasIt.Andor.Budget.Application;

public class AccountQueriesService()
    : IAccountQueriesService
{
    public Task<AccountOutput?> GetByIdAsync(AccountId id,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
