using AdasIt.Andor.Budgets.Domain.Categories.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Categories;

public class CategoryValidator : DefaultValidator<Category, CategoryId>, ICategoryValidator
{
    protected sealed override async Task DefaultValidationsAsync(Category entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.Name.NotNull(), notifications);
        
        AddNotification(entity.Description.NotNull(), notifications);
        
        AddNotification(entity.StartDate.NotDefaultDateTime(), notifications);
        
        AddNotification(entity.DeactivationDate.NotDefaultDateTime(), notifications);
    }
}
