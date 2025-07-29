using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.DomainQueries;

public record SearchConfigurationInput : SearchInput
{
    public string Name { get; set; } = "";
    public ConfigurationState[] States { get; set; } = [];
}
