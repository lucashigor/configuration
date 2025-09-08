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

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected Invite()
#pragma warning restore CS8618, CS9264
    {
        // For EF
    }

    private Invite(string email, User inviting, User? guest,
        InviteStatus status, Account account)
    {
            Email = email;
            InvitingId = inviting.Id;
            Inviting = inviting;
            GuestId = guest?.Id;
            Guest = guest;
            Status = status;
            AccountId = account.Id;
            Account = account;
    }

    public static async Task<(DomainResult, Invite?)> NewAsync(string email, User inviting, User? guest, Account account,
        IInviteValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new Invite(email, inviting, guest, InviteStatus.Pending, account);
        
        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
