using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public class AccountValidator : IAccountValidator
{
    public Task<List<Notification>> ValidateCreationAsync(string value, string description, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Notification>> ValidateUpdateAsync(Account current, string name, string description, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
