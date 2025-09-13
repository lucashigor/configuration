using AdasIt.Andor.Budgets.Domain.Invites.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Invites;

public class Invite : Entity<InviteId>
{
    public Email Email { get; private set; }
    public UserId InvitingId { get; private set; }
    public User Inviting { get; private set; }
    public UserId GuestId { get; private set; }
    public User Guest { get; private set; }
    public InviteStatus Status { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected Invite()
#pragma warning restore CS8618, CS9264
    {
        // For EF
    }

    private Invite(Email email, User inviting, User? guest,
        InviteStatus status)
    {
        Email = email;
        InvitingId = inviting.Id;
        Inviting = inviting;

        if (guest != null)
        {
            GuestId = guest.Id;
            Guest = guest;
        }

        Status = status;
    }

    public static async Task<(DomainResult, Invite?)> NewAsync(Email email, User inviting, User? guest,
        IInviteValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Invite(email, inviting, guest, InviteStatus.Pending);

        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
