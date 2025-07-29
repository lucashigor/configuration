using AdasIt.Andor.DomainQueries.ResearchableRepository;

namespace AdasIt.Andor.Configurations.DomainQueries;

public interface IQueriesConfigurationRepository<TOutput> :
    IResearchableRepository<TOutput, Guid, SearchConfigurationInput>
{
    Task<List<TOutput>?> GetByNameAndStatusAsync(SearchConfigurationInput search,
        CancellationToken cancellationToken);

    Task<TOutput?> GetActiveByNameAsync(string name,
        CancellationToken cancellationToken);
}

