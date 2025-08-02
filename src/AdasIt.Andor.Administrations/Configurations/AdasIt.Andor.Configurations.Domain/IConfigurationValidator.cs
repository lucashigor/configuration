using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public interface IConfigurationValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name, string value, string description, DateTime startDate, DateTime? expireDate,
        CancellationToken cancellationToken);

    Task<List<Notification>> ValidateUpdateAsync(Configuration existing, string name, string value, string description, DateTime startDate,
        DateTime? expireDate, CancellationToken cancellationToken);
}
