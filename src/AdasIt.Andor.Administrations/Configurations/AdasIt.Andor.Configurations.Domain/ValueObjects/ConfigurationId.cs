using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Configurations.Domain.ValueObjects;
public record struct ConfigurationId(Guid Value) : IId<ConfigurationId>
{
    public static ConfigurationId New() => new ConfigurationId(Guid.NewGuid());

    public static ConfigurationId Load(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
        {
            throw new ArgumentException(DefaultsErrorsMessages.InvalidGuid, nameof(value));
        }
        return new ConfigurationId(guid);
    }

    public static ConfigurationId Load(Guid value) => new(value);

    public override readonly string ToString() => Value.ToString();


    public static implicit operator ConfigurationId(Guid value) => new(value);

    public static implicit operator Guid(ConfigurationId id) => id.Value;
}
