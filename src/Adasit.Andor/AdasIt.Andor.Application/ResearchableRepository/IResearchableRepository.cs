namespace AdasIt.Andor.DomainQueries.ResearchableRepository;

public interface IResearchableRepository<TEntity, TEntityId, TSearchInput>
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);

    Task<SearchOutput<TEntity>> SearchAsync(TSearchInput input, CancellationToken cancellationToken);
}
