using AdasIt.Andor.Budgets.Domain.Accounts.Events;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.Invites;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public class Account : AggregateRoot<AccountId>
{
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Currency Currency { get; private set; }
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

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected Account()
#pragma warning restore CS8618, CS9264
    {
    }

    private Account(Name name,
        Description description,
        Currency currency,
        AccountStatus status)
    {
        Name = name;
        Description = description;
        Currency = currency;
        Status = status;
    }

    public static async Task<(DomainResult, Account?)> NewAsync(Name name,
        Description description,
        Currency currency,
        IAccountValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Account(name, description, currency, AccountStatus.Active);

        var result = await entity.ValidateAsync(validator, cancellationToken);

        entity.RaiseDomainEvent(AccountCreated.FromConfiguration(entity));

        return result;
    }

    public DomainResult SetCategoriesAvailable(IEnumerable<Category> categories)
    {
        var categoriesList = categories.ToList();

        _categories = categoriesList;
        _subCategories = categoriesList.SelectMany(c => c.SubCategories).ToList();

        return DomainResult.Success();
    }

    public DomainResult SetPaymentMethodsAvailable(IEnumerable<PaymentMethod> paymentMethods)
    {
        _paymentMethods = paymentMethods.ToList();

        return DomainResult.Success();
    }

    public DomainResult AddFinancialMovement(
        DateTime date,
        string name,
        SubCategoryId subCategoryId,
        int status,
        PaymentMethodId paymentMethodId,
        decimal value, IAccountValidator validator)
    {
        var subCategory = _subCategories.SingleOrDefault(sc => sc.Id == subCategoryId);

        if (subCategory is null)
        {
            //return DomainResult.Failure($"Sub category with id {command.SubCategoryId} not found in account");
        }

        var paymentMethod = _paymentMethods.SingleOrDefault(pm => pm.Id == paymentMethodId);

        if (paymentMethod is null)
        {
            //return DomainResult.Failure($"Payment method with id {command.PaymentMethodId} not found in account");
        }

        return DomainResult.Success();
    }
}
