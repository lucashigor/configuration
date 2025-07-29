using AdasIt.Andor.DomainQueries;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace AdasIt.Andor.InfrastructureQueries;
public static class GlobalQueryExtensions
{
    public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityType)
    {
        var method = typeof(GlobalQueryExtensions)
            .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)?
            .MakeGenericMethod(entityType.ClrType);

        if (method == null)
            return;

        var filter = method.Invoke(null, []);
        entityType.SetQueryFilter((LambdaExpression)filter!);
        entityType.AddIndex(entityType.FindProperty(nameof(ISoftDeletableEntity.IsDeleted))!);
    }

    private static Expression<Func<TEntity, bool>> GetSoftDeleteFilter<TEntity>()
        where TEntity : class, ISoftDeletableEntity
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}
