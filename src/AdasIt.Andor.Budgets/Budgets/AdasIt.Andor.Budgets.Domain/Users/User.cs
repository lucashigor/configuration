using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public CurrencyId PreferredCurrencyId { get; private set; }
    public LanguageId PreferredLanguageId { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    private User()
#pragma warning restore CS8618, CS9264
    {
    }

    private User(Name firstName, Name lastName, Email email, CurrencyId preferredCurrencyId,
        LanguageId preferredLanguageId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PreferredCurrencyId = preferredCurrencyId;
        PreferredLanguageId = preferredLanguageId;
    }

    public static async Task<(DomainResult, User?)> NewAsync(Name firstName, Name lastName,
        Email email, CurrencyId preferredCurrencyId, LanguageId preferredLanguageId,
        IUserValidator validator, CancellationToken cancellationToken)
    {
        var entity = new User(firstName, lastName, email, preferredCurrencyId, preferredLanguageId);

        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
