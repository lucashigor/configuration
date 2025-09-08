using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Users;

public class UserValidator : DefaultValidator<User, UserId>, IUserValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        User entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.FirstName.NotNull(), notifications);
        AddNotification(entity.LastName.NotNull(), notifications);
    }
}
