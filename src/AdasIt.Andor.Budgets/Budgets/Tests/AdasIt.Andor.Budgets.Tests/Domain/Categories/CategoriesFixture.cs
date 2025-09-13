using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.TestsUtil;

namespace AdasIt.Andor.Budgets.Tests.Domain;

internal static class CategoriesFixture
{
    public static Category GetValidDepositCategory()
        => GetValidCategory(MovementType.MoneyDeposit);

    public static Category GetValidCategory(MovementType movementType)
    {
        SubCategory subCategory;

        if (movementType == MovementType.MoneyDeposit)
        {
            subCategory = SubCategoryFixture.GetValidDepositSubCategory();
        }
        else
        {
            subCategory = SubCategoryFixture.GetValidSpendingSubCategory();
        }

        var category = GeneralFixture.CreateInstanceAndSetProperties<Category>(
            new Dictionary<string, object>()
        {
            { nameof(Category.Id), CategoryId.New() },
            { nameof(Category.Name), GeneralFixture.GetValidName() },
            { nameof(Category.Description), GeneralFixture.GetValidDescription() },
            { nameof(Category.StartDate), DateTime.Now },
            { nameof(Category.DeactivationDate), null },
            { nameof(Category.Type), movementType },
            { nameof(Category.SubCategories), new List<SubCategory>() { subCategory } },
        });

        return category;
    }
}
