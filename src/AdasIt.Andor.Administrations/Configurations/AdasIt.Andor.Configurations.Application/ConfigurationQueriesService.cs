using AdasIt.Andor.Configurations.Application.Interfaces;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.Application;

public class ConfigurationQueriesService(
    IQueriesConfigurationRepository<ConfigurationOutput> queriesConfigurationRepository)
    : IConfigurationQueriesService
{
    public Task<ConfigurationOutput?> GetByIdAsync(ConfigurationId id,
        CancellationToken cancellationToken)
        => queriesConfigurationRepository.GetByIdAsync(id, cancellationToken);

    public Task<SearchOutput<ConfigurationOutput>> SearchAsync(SearchConfigurationInput input,
        CancellationToken cancellationToken)
        => queriesConfigurationRepository.SearchAsync(input, cancellationToken);
}
