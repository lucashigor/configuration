using AdasIt.Andor.Configurations.Domain.ValueObjects;

namespace AdasIt.Andor.Configurations.Domain.Repository;

public record SearchConfigurationInput(string Name, ConfigurationState[] States);
