using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Domain
{
    public interface IConfigurationValidator
    {
        List<Notification> ValidateCreation(string name, string value, string description, DateTime startDate, DateTime? expireDate);
        List<Notification> ValidateUpdate(Configuration existing, string name, string value, string description, DateTime startDate, DateTime? expireDate);
    }
}
