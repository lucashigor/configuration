using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Domain.SeedWork.Repositories;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Repository;

public interface ICommandsAccountRepository :
    ICommandRepository<Account, AccountId>
{
    Task<Currency> GetDefaultCurrency(CancellationToken cancellationToken);
}
