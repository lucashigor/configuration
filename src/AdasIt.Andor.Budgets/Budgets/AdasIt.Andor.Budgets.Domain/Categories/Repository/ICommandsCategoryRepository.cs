using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Domain.SeedWork.Repositories;

namespace AdasIt.Andor.Budgets.Domain.Categories.Repository;

public interface ICommandsCategoryRepository :
    ICommandRepository<Category, CategoryId>
{
    Task<IEnumerable<Category>> GetDefaultCategories(CancellationToken cancellationToken);
    
}