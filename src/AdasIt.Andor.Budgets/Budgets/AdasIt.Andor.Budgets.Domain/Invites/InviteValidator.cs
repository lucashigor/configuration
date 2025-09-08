using AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Invites;

public class InviteValidator : DefaultValidator<Invite, InviteId>,  IInviteValidator
{
    protected sealed override async Task DefaultValidationsAsync(
        Invite entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        await base.DefaultValidationsAsync(entity, notifications, cancellationToken);
        
        AddNotification(entity.Email.NotNullOrEmptyOrWhiteSpace(), notifications);
        AddNotification(entity.Email.BetweenLength(3, 70), notifications);
    }
}
