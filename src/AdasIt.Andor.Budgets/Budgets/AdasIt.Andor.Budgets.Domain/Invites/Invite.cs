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

    private DomainResult SetValues(InviteId id,
        string email,
        AccountId accountId,
        InviteStatus inviteStatus,
        UserId invitingId,
        UserId? guestId)
    {

        if (Notifications.Count > 1)
        {
            return Validate();
        }

        Email = email;
        InvitingId = invitingId;
        GuestId = guestId;
        Status = inviteStatus;
        AccountId = accountId;

        var result = Validate();

        return result;
    }

    public static (DomainResult, Invite?) New(
        string email,
        AccountId accountId,
        UserId Inviting)
    {
        var entity = new Invite();

        var result = entity.SetValues(InviteId.New(), email, accountId, InviteStatus.Pending, Inviting, null);

        if (result.IsFailure)
        {
            return (result, null);
        }

        entity.RaiseDomainEvent(InviteCreatedDomainEvent.FromAggregator(entity));

        return (result, entity);
    }

    public (DomainResult, Invite?) GuestFound(
        UserId guest)
    {
        var result = SetValues(Id, Email, AccountId, InviteStatus.Pending, InvitingId, guest);

        if (result.IsFailure)
        {
            return (result, null);
        }

        RaiseDomainEvent(GuestFoundDomainEvent.FromAggregator(this));

        return (result, this);
    }

    public (DomainResult, Invite?) GuestNotFound()
    {
        var result = SetValues(Id, Email, AccountId, InviteStatus.Pending, InvitingId, null);

        if (result.IsFailure)
        {
            return (result, null);
        }

        RaiseDomainEvent(GuestNotFoundDomainEvent.FromAggregator(this));

        return (result, this);
    }

    public (DomainResult, Invite?) InvitationMade()
    {
        RaiseDomainEvent(InvitationMadeDomainEvent.FromAggregator(this));

        return (DomainResult.Success(), this);
    }

    public (DomainResult, Invite?) InvitationAnswered(bool accepeted)
    {
        var status = accepeted ? InviteStatus.Accepted : InviteStatus.Refused;

        var result = SetValues(Id, Email, AccountId, status, InvitingId, GuestId);

        if (result.IsFailure)
        {
            return (result, null);
        }

        RaiseDomainEvent(InvitationAnsweredDomainEvent.FromAggregator(this));

        return (result, this);
    }
}
