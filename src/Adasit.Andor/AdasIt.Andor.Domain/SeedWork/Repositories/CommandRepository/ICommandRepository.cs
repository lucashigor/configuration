using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdasIt.Andor.Domain.SeedWork.Repositories.CommandRepository;

public interface ICommandRepository<TEntity, TEntityId>
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    Task PersistAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);
}
