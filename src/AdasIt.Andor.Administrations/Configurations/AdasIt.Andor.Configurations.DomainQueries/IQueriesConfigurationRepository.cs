using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.DomainQueries;

public interface IQueriesConfigurationRepository :
    IResearchableRepository<ConfigurationOutput, Guid, SearchConfigurationInput>
{
    Task<List<ConfigurationOutput>?> GetByNameAndStatesAsync(SearchConfigurationInput search,
        CancellationToken cancellationToken);

    Task<ConfigurationOutput?> GetActiveByNameAsync(string name,
        CancellationToken cancellationToken);
}

