using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users.Events;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;
using System.Net.Mail;

namespace AdasIt.Andor.Budgets.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public MailAddress Email { get; private set; }
    public CurrencyId PreferredCurrencyId { get; set; }
    public LanguageId PreferredLanguageId { get; set; }

    private DomainResult SetValues(
        MailAddress email,
        string firstName,
        string lastName,
        CurrencyId preferredCurrencyId,
        LanguageId preferredLanguageId)
    {

        if (Notifications.Count > 1)
        {
            return Validate();
        }

        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PreferredCurrencyId = preferredCurrencyId;
        PreferredLanguageId = preferredLanguageId;

        var result = Validate();

        return result;
    }

    public static (DomainResult, User?) New(UserId userId, MailAddress email,
        string firstName,
        string lastName,
        CurrencyId preferredCurrencyId,
        LanguageId preferredLanguageId)
    {
        var entity = new User();

        var result = entity.SetValues(
            email,
            firstName,
            lastName,
            preferredCurrencyId,
           preferredLanguageId);

        if (result.IsFailure)
        {
            return (result, null);
        }

        entity.RaiseDomainEvent(UserCreatedDomainEvent.FromAggregateRoot(entity));

        return (result, entity);
    }
}
