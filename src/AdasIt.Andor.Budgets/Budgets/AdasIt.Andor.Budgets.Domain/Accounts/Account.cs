using AdasIt.Andor.Budgets.Domain.Accounts.Commands;
using AdasIt.Andor.Budgets.Domain.Accounts.Events;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.Invites;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public class Account : AggregateRoot<AccountId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Currency? Currency { get; private set; }
    public AccountStatus Status { get; private set; } = AccountStatus.Undefined;

    public IReadOnlyCollection<Category> Categories => [.. _categories];
    private ICollection<Category> _categories { get; set; } = new HashSet<Category>();
    public IReadOnlyCollection<SubCategory> SubCategories => [.. _subCategories];
    private ICollection<SubCategory> _subCategories { get; set; } = new HashSet<SubCategory>();
    public IReadOnlyCollection<PaymentMethod> PaymentMethods => [.. _paymentMethods];
    private ICollection<PaymentMethod> _paymentMethods { get; set; } = new HashSet<PaymentMethod>();
    public IReadOnlyCollection<User> Members => [.. _members];
    private ICollection<User> _members { get; set; } = new HashSet<User>();
    public IReadOnlyCollection<Invite> Invites => [.. _invites];
    private ICollection<Invite> _invites { get; set; } = new HashSet<Invite>();

    public Account()
    {
    }

    private Account(string name,
        string description,
        Currency currency,
        AccountStatus status)
    {
        Name = name;
        Description = description;
        Currency = currency;
        Status = status;
    }

    public static async Task<(DomainResult, Account?)> NewAsync(string name,
        string description,
        Currency currency,
        IAccountValidator accountValidator,
        CancellationToken cancellationToken)
    {
        var entity = new Account(name, description, currency, AccountStatus.Active);

        var notifications = await accountValidator.ValidateCreationAsync(entity.Name, entity.Description, entity.Currency, entity.Status, cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        entity.RaiseDomainEvent(AccountCreated.FromConfiguration(entity));

        return (result, entity);
    }

    public DomainResult SetCategoriesAvailable(Deposit deposit)
    {
        this.RaiseDomainEvent(new DepositRegistered());

        return DomainResult.Success();
    }

    public DomainResult SetPaymentMethodsAvailable(Deposit deposit)
    {
        this.RaiseDomainEvent(new DepositRegistered());

        return DomainResult.Success();
    }

    public DomainResult Deposit(Deposit deposit)
    {
        this.RaiseDomainEvent(new DepositRegistered());

        return DomainResult.Success();
    }

    public DomainResult Withdrawal(Withdrawal withdrawal)
    {
        this.RaiseDomainEvent(new WithdrawalRegistered());
        return DomainResult.Success();
    }
}
