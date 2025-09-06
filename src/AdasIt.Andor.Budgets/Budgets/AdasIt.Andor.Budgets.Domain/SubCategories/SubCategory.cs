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
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime DeactivationDate { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public Category Category { get; private set; }
    public PaymentMethodId DefaultPaymentMethodId { get; private set; }
    public PaymentMethod DefaultPaymentMethod { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private SubCategory()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // For EF
    }

    private SubCategory(string name,
        string description,
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

    public static async Task<(DomainResult, SubCategory?)> NewAsync(string name,
        string description,
        DateTime startDate,
        DateTime deactivationDate,
        Category category,
        PaymentMethod defaultPaymentMethod,
        ISubCategoryValidator _validator,
        CancellationToken cancellationToken)
    {
        var entity = new SubCategory(name, description, startDate, deactivationDate,
            category, defaultPaymentMethod);

        var notifications = await _validator.ValidateCreationAsync(entity.Name,
            entity.Description, entity.StartDate, entity.DeactivationDate, entity.Category,
            entity.DefaultPaymentMethod, cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
