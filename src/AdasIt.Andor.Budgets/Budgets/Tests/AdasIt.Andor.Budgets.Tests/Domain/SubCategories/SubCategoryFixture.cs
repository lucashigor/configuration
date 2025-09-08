using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using NSubstitute;

namespace AdasIt.Andor.Budgets.Tests.Domain;

internal static class SubCategoryFixture
{
    public static SubCategory GetValidDepositSubCategory()
        => GetValidSubCategory(MovementType.MoneyDeposit);
    
    public static SubCategory GetValidSpendingSubCategory()
        => GetValidSubCategory(MovementType.MoneySpending);
    
    private static SubCategory GetValidSubCategory(MovementType movementType)
    {
        var category = CategoriesFixture.GetValidCategory(movementType);
        var paymentMethod = PaymentMethodFixture.GetValidPaymentMethod(movementType);
        
        var subCategory = GeneralFixture.CreateInstanceAndSetProperties<SubCategory>(
            new Dictionary<string, object>()
            {
                { nameof(SubCategory.Id), SubCategoryId.New() },
                { nameof(SubCategory.Name), GetSubCategoryValidName() },
                { nameof(SubCategory.Description), GetSubCategoryValidDescription() },
                { nameof(SubCategory.StartDate), DateTime.Now },
                { nameof(SubCategory.DeactivationDate), null },
                { nameof(SubCategory.DefaultPaymentMethodId), paymentMethod.Id },
                { nameof(SubCategory.DefaultPaymentMethod), paymentMethod },
                { nameof(SubCategory.CategoryId), category.Id },
                { nameof(SubCategory.Category), category },
            });
        
        return subCategory;
    }
    
    private static string GetSubCategoryValidName()
        => GeneralFixture.GetStringRightSize(3,50);
    
    private static string GetSubCategoryValidDescription()
        => GeneralFixture.GetStringRightSize(3, 255);
}
