using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public class SubCategoryValidator : ISubCategoryValidator
{
    public async Task<List<Notification>> ValidateCreationAsync(string name,
        string description,
        DateTime startDate,
        DateTime deactivationDate,
        Category category,
        PaymentMethod defaultPaymentMethod,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = new();

        await DefaultValidationsAsync(name, description, startDate,
        deactivationDate, category, defaultPaymentMethod, notifications, cancellationToken);

        return notifications;
    }

    private Task DefaultValidationsAsync(
        string name,
        string description,
        DateTime startDate,
        DateTime deactivationDate,
        Category category,
        PaymentMethod defaultPaymentMethod,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(name.BetweenLength(3, 70), notifications);

        AddNotification(description.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(description.BetweenLength(3, 255), notifications);

        AddNotification(startDate.NotNull(), notifications);

        AddNotification(deactivationDate.NotNull(), notifications);

        AddNotification(category.NotNull(), notifications);

        AddNotification(defaultPaymentMethod.NotNull(), notifications);

        return Task.CompletedTask;
    }

    private static void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}
