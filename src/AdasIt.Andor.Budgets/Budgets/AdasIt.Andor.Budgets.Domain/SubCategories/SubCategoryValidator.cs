using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public class SubCategoryValidator : DefaultValidator<SubCategory, SubCategoryId>, ISubCategoryValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        SubCategory entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.Name.NotNull(), notifications);

        AddNotification(entity.Description.NotNull(), notifications);

        AddNotification(entity.StartDate.NotNull(), notifications);

        AddNotification(entity.Category.NotNull(), notifications);

        AddNotification(entity.DefaultPaymentMethod.NotNull(), notifications);
    }
}
