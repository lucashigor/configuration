using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain.SeedWork.Repositories.ResearchableRepository;

namespace AdasIt.Andor.Configurations.Domain.Repository;

public interface IQueriesConfigurationRepository :
    IResearchableRepository<Configuration, ConfigurationId, SearchInput>
{
    Task<List<Configuration>?> GetByNameAndStatusAsync(SearchConfigurationInput search,
        CancellationToken cancellationToken);

    Task<Configuration?> GetActiveByNameAsync(string name,
        CancellationToken cancellationToken);
}
