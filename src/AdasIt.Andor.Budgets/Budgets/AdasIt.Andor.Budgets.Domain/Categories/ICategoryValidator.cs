using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public interface ICategoryValidator : IDefaultValidator<Category, CategoryId>
{
}
