using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Budgets.Domain.Categories.Repository;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.Repository;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts
{
    public class AccountFactory(
        ICommandsAccountRepository commandsAccountRepository,
        ICommandsPaymentMethodRepository commandsPaymentMethodRepository,
        ICommandsCategoryRepository commandsCategoryRepository,
        IAccountValidator accountValidator)
    {
        public async Task<(DomainResult, Account?)> CreateAsync(string name,
            string description,
            CancellationToken cancellationToken)
        {
            var currency = await commandsAccountRepository.GetDefaultCurrency(cancellationToken);
            
            var categories = await commandsCategoryRepository.GetDefaultCategories(cancellationToken);
            
            var paymentMethods = await commandsPaymentMethodRepository
                .GetDefaultPaymentMethods(cancellationToken);

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
