using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
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
    public Category Category { get; private set; }
    public MovementType Type => Category.Type;
    public PaymentMethodId DefaultPaymentMethodId { get; private set; }
    public PaymentMethod DefaultPaymentMethod { get; private set; }

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
        Category category,
        PaymentMethod defaultPaymentMethod)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DeactivationDate = deactivationDate;
        Category = category;
        CategoryId = category.Id;
        DefaultPaymentMethod = defaultPaymentMethod;
        DefaultPaymentMethodId = defaultPaymentMethod.Id;
    }

    public static async Task<(DomainResult, SubCategory?)> NewAsync(Name name,
        Description description,
        DateTime startDate,
        DateTime deactivationDate,
        Category category,
        PaymentMethod defaultPaymentMethod,
        ISubCategoryValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new SubCategory(name, description, startDate, deactivationDate,
            category, defaultPaymentMethod);
        
        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
