using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts
{
    public class AccountFactory(
        IDefaultsProvider defaultsProvider,
        IAccountValidator accountValidator)
    {
        public async Task<(DomainResult, Account?)> CreateAsync(string name,
            string description,
            CancellationToken cancellationToken)
        {
            var currency = await defaultsProvider.GetDefaultCurrency(cancellationToken);

            var categories = await defaultsProvider.GetDefaultCategories(cancellationToken);

            var paymentMethods = await defaultsProvider.GetDefaultPaymentMethods(cancellationToken);

            var (result, account) = await Account.NewAsync(name, description, currency, accountValidator, cancellationToken);

            if (!result.IsSuccess)
            {
                return (result, null);
            }

            account?.SetCategoriesAvailable(categories);

            account?.SetPaymentMethodsAvailable(paymentMethods);

            return (result, account);
        }
    }
}
