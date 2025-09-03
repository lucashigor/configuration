using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.FinancialMovements.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public class Category : Entity<CategoryId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public MovementType Type { get; private set; } = MovementType.Undefined;
    public ICollection<SubCategory> SubCategories { get; private set; }
    public int? DefaultOrder { get; private set; }

    public Category()
    {
        Id = CategoryId.New();
        Name = string.Empty;
        Description = string.Empty;
        SubCategories = [];
    }

    private DomainResult SetValues(CategoryId id,
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
        DefaultOrder = order;

        var result = Validate();

        return result;
    }

    public static (DomainResult, Category?) New(
        string name,
        int order)
    {
        var entity = new Category();

        var result = entity.SetValues(CategoryId.New(), name, order);

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
