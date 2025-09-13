using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;

namespace AdasIt.Andor.Budget.Application.Interfaces;

public interface IAccountQueriesService
{
    Task<AccountOutput?> GetByIdAsync(AccountId id, CancellationToken cancellationToken);
}
