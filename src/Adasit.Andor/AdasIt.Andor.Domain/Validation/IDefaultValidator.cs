using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Domain.Validation;

public interface IDefaultValidator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    Task<List<Notification>> ValidateCreationAsync(TEntity entity,
        CancellationToken cancellationToken);
}