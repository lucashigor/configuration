using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Users;

public interface IUserValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name, CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(string name, CancellationToken cancellationToken);
}
