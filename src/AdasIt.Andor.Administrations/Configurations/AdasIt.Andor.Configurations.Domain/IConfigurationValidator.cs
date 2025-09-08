using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain;

public interface IConfigurationValidator : IDefaultValidator<Configuration, ConfigurationId>
{
    Task<List<Notification>> ValidateUpdateAsync(Configuration existing, string name, string value, string description,
        DateTime startDate, DateTime? expireDate, CancellationToken cancellationToken);
}
