using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdasIt.Andor.Domain.SeedWork.Repositories.ResearchableRepository;

public interface IResearchableRepository<TEntity, TEntityId, TSearchInput>
    where TEntity : Entity<TEntityId>
    where TSearchInput : SearchInput
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);

    Task<SearchOutput<TEntity>> SearchAsync(TSearchInput input, CancellationToken cancellationToken);
}
