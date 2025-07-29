using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.Application.Interfaces;

public interface IConfigurationQueriesService
{
    Task<ConfigurationOutput?> GetByIdAsync(ConfigurationId id, CancellationToken cancellationToken);

    Task<SearchOutput<ConfigurationOutput>> SearchAsync(SearchConfigurationInput input, CancellationToken cancellationToken);
}
