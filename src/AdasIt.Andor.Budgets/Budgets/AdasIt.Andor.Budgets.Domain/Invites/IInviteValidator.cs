using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Invites;

public interface IInviteValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name, CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(string name, CancellationToken cancellationToken);
}
