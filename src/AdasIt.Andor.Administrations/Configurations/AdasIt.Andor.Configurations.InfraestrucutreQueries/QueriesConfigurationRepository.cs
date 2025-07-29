using AdasIt.Andor.Configurations.InfrastructureQueries.Context;
using AdasIt.Andor.DomainQueries;
using AdasIt.Andor.DomainQueries.ResearchableRepository;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AdasIt.Andor.Configurations.InfrastructureQueries;

public class QueriesConfigurationRepository(ConfigurationContext context) :
    QueryHelper<ConfigurationOutput, Guid>(context),
    IQueriesConfigurationRepository<ConfigurationOutput>
{
    public Task<SearchOutput<ConfigurationOutput>> SearchAsync(SearchConfigurationInput input, CancellationToken cancellationToken)
    {
        Expression<Func<ConfigurationOutput, bool>> where = x => true;

        if (!string.IsNullOrWhiteSpace(input.Search))
            where = x => x.Name.Contains(input.Search, StringComparison.CurrentCultureIgnoreCase);

        var items = GetManyPaginated(where,
            input.OrderBy,
            input.Order,
            input.Page,
            input.PerPage,
            out var total)
            .ToList();

        return Task.FromResult(new SearchOutput<ConfigurationOutput>(input.Page, input.PerPage, total, items!));
    }

    public async Task<List<ConfigurationOutput>?> GetByNameAndStatusAsync(SearchConfigurationInput search,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();
        query = GetWhere(query, search);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<ConfigurationOutput?> GetActiveByNameAsync(string name, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();
        query = GetWhere(query, new SearchConfigurationInput() { Name = name, States = [ConfigurationState.Active] });

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    private static IQueryable<ConfigurationOutput> GetWhere(IQueryable<ConfigurationOutput> sourceQuery, SearchConfigurationInput search)
    {
        if (!string.IsNullOrEmpty(search.Name))
        {
            sourceQuery = sourceQuery.Where(x => x.Name.Equals(search.Name));
        }

        if (search.States.Contains(ConfigurationState.Expired))
        {
            sourceQuery = sourceQuery.Where(x => x.ExpireDate > DateTime.UtcNow);
        }

        if (!search.States.Contains(ConfigurationState.Awaiting))
        {
            sourceQuery = sourceQuery.Where(x => x.StartDate < DateTime.UtcNow);
        }

        if (!search.States.Contains(ConfigurationState.Active))
        {
            sourceQuery = sourceQuery.Where(x => x.StartDate < DateTime.UtcNow && (x.ExpireDate == null || x.ExpireDate > DateTime.UtcNow));
        }

        return sourceQuery;
    }
}
