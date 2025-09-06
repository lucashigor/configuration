using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Domain.SeedWork;
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Category()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // For EF
    }

    public static async Task<(DomainResult, Category?)> NewAsync(string name,
        ICategoryValidator currencyValidator,
        CancellationToken cancellationToken)
    {
        var entity = new Category();

        var notifications = await currencyValidator.ValidateCreationAsync(entity.Name,
            cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
