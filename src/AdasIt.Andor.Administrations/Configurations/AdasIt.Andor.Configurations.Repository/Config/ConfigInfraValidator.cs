using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.InfrastructureCommands.Config;

internal class ConfigInfraValidator : IConfigurationValidator
{
    private readonly List<Notification> notifications = new();

    public List<Notification> ValidateCreation(string name, string value, string description, DateTime startDate, DateTime? expireDate)
    {
        return notifications;
    }

    public List<Notification> ValidateUpdate(Configuration existing, string name, string value, string description, DateTime startDate, DateTime? expireDate)
    {
        return notifications;
    }
}
