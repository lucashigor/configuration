using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public class SubCategory : Entity<SubCategoryId>
{
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public MovementType Type { get; private set; }
    public PaymentMethodId DefaultPaymentMethodId { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected SubCategory()
#pragma warning restore CS8618, CS9264
    {
        // For EF
    }

    private SubCategory(Name name,
        Description description,
        DateTime startDate,
        DateTime deactivationDate,
        PaymentMethodId defaultPaymentMethod,
        MovementType type,
        CategoryId categoryId)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DeactivationDate = deactivationDate;
        Type = type;
        CategoryId = categoryId;
        DefaultPaymentMethodId = defaultPaymentMethod;
    }

    public static async Task<(DomainResult, SubCategory?)> NewAsync(Name name,
        Description description,
        DateTime startDate,
        DateTime deactivationDate,
        CategoryId categoryId,
        PaymentMethodId defaultPaymentMethod,
        MovementType type,
        ISubCategoryValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new SubCategory(name, description, startDate, deactivationDate,
            defaultPaymentMethod, type, categoryId);

        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
