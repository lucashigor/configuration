using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public class Category : Entity<CategoryId>
{
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public MovementType Type { get; private set; }
    public ICollection<SubCategory> SubCategories { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected Category()
#pragma warning restore CS8618, CS9264
    {
    }

    private Category(string name, string description, DateTime startDate,
        DateTime? deactivationDate, MovementType type)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DeactivationDate = deactivationDate;
        Type = type;

        SubCategories = [];
    }

    public static async Task<(DomainResult, Category?)> NewAsync(string name, string description, DateTime startDate,
        DateTime? deactivationDate, MovementType type, ICategoryValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Category(name, description, startDate, deactivationDate, type);
        
        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
