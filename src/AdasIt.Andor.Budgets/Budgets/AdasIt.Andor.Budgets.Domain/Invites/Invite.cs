using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Invites;

public class Invite : Entity<InviteId>
{
    public string Email { get; private set; }
    public UserId InvitingId { get; private set; }
    public User Inviting { get; private set; }
    public UserId? GuestId { get; private set; }
    public User? Guest { get; private set; }
    public InviteStatus Status { get; private set; }
    public AccountId AccountId { get; private set; }
    public Account? Account { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Invite()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // For EF
    }

    public static async Task<(DomainResult, Invite?)> NewAsync(string name,
        IInviteValidator currencyValidator,
        CancellationToken cancellationToken)
    {
        var entity = new Invite();

        var notifications = await currencyValidator.ValidateCreationAsync(entity.Email, cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
