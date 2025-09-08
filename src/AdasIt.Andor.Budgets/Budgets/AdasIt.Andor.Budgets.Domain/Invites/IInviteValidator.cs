using AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Invites;

public interface IInviteValidator :  IDefaultValidator<Invite, InviteId>
{
}
