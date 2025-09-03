using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public class SubCategory : Entity<SubCategoryId>
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public DateTime? StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public CategoryId? CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public PaymentMethodId? DefaultPaymentMethodId { get; private set; }
    public PaymentMethod? DefaultPaymentMethod { get; private set; }
    public int? DefaultOrder { get; private set; }

    private DomainResult SetValues(SubCategoryId id,
        string name,
        int order)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace());
        AddNotification(name.BetweenLength(3, 70));

        if (Notifications.Count > 1)
        {
            return Validate();
        }

        Name = name;

        var result = Validate();

        return result;
    }

    public static (DomainResult, SubCategory?) New(
        string name,
        int order)
    {
        var entity = new SubCategory();

        var result = entity.SetValues(SubCategoryId.New(), name, order);

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
