using AdasIt.Andor.Budgets.Domain.Accounts.Repository;

namespace AdasIt.Andor.Budgets.Domain.Accounts
{
    public class AccountFactory
    {
        private readonly ICommandsAccountRepository _commandsAccountRepository;
        private readonly IAccountValidator _accountValidator;

        public AccountFactory(ICommandsAccountRepository commandsAccountRepository,
            IAccountValidator accountValidator)
        {
            _commandsAccountRepository = commandsAccountRepository;
            _accountValidator = accountValidator;
        }

        public async Task<Account> CreateAsync(string name,
            string description,
            CancellationToken cancellationToken)
        {
            var currency = await _commandsAccountRepository.GetDefaultCurrency(cancellationToken);
            var categories = await _commandsAccountRepository.GetDefaultCategories(cancellationToken);
            var paymentMethods = await _commandsAccountRepository.GetDefaultPaymentMethods(cancellationToken);

            var (_, account) = await Account.NewAsync(name, description, currency, _accountValidator, cancellationToken);

            account?.SetCategoriesAvailable(categories);

            account?.SetPaymentMethodsAvailable(paymentMethods);

            return account;
        }
    }
}
