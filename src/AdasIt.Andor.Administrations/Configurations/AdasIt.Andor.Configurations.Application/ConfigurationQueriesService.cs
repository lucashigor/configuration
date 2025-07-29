using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationQueriesService : IConfigurationQueriesService
{
    private readonly IQueriesConfigurationRepository<ConfigurationOutput> _queriesConfigurationRepository;

    public ConfigurationQueriesService(IQueriesConfigurationRepository<ConfigurationOutput> queriesConfigurationRepository)
    {
        _queriesConfigurationRepository = queriesConfigurationRepository;
    }

    public Task<ConfigurationOutput?> GetByIdAsync(ConfigurationId id,
        CancellationToken cancellationToken)
        => _queriesConfigurationRepository.GetByIdAsync(id, cancellationToken);

    public Task<SearchOutput<ConfigurationOutput>> SearchAsync(SearchConfigurationInput input,
        CancellationToken cancellationToken)
        => _queriesConfigurationRepository.SearchAsync(input, cancellationToken);
}
