using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public interface ICategoryValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name,
        CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(string name,
        CancellationToken cancellationToken);
}
