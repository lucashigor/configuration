using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public interface ISubCategoryValidator : IDefaultValidator<SubCategory, SubCategoryId>
{
}
