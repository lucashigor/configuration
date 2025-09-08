using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using NSubstitute;

namespace AdasIt.Andor.Budgets.Tests.Domain;

internal static class CategoriesFixture
{
    public static Category GetValidDepositCategory()
        => GetValidCategory(MovementType.MoneyDeposit);
    
    public static Category GetValidCategory(MovementType movementType)
    {
        var category = GeneralFixture.CreateInstanceAndSetProperties<Category>(
            new Dictionary<string, object>()
        {
            { nameof(Category.Id), CategoryId.New() },
            { nameof(Category.Name), GeneralFixture.GetValidName() },
            { nameof(Category.Description), GeneralFixture.GetValidDescription() },
            { nameof(Category.StartDate), DateTime.Now },
            { nameof(Category.DeactivationDate), null },
            { nameof(Category.Type), movementType },
        });
        
        return category;
    }
}
