using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.InfrastructureCommands.Config;

internal class ConfigInfraValidator : IConfigurationValidator
{
    private readonly List<Notification> _notifications = new();

    public Task<List<Notification>> ValidateCreationAsync(string name, string value, string description, DateTime startDate, DateTime? expireDate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_notifications);
    }

    public Task<List<Notification>> ValidateUpdateAsync(Configuration existing, string name, string value, string description, DateTime startDate, DateTime? expireDate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_notifications);
    }
}
