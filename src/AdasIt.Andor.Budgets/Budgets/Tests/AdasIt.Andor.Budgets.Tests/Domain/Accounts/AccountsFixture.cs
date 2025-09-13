using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.TestsUtil;

namespace AdasIt.Andor.Budgets.Tests.Domain.Accounts;

internal static class AccountsFixture
{
    public static Account GetValidAccount()
    {
        var account = GeneralFixture.CreateInstanceAndSetProperties<Account>(
            new Dictionary<string, object>()
        {
            { nameof(Account.Id), AccountId.New() },
            { nameof(Account.Name), GeneralFixture.GetValidName() },
            { nameof(Account.Description), GeneralFixture.GetValidDescription() },
            { nameof(Account.Status), AccountStatus.Active },
        });

        var category = CategoriesFixture.GetValidDepositCategory();

        account.SetCategoriesAvailable(new List<Category>()
        {
            CategoriesFixture.GetValidDepositCategory(),
        });

        return account;
    }

}
