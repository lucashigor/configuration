using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public interface ICategoryValidator : IDefaultValidator<Category, CategoryId>
{
}
