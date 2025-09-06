using AdasIt.Andor.Budgets.Domain.Currencies.ValueObjects;
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // For EF
    }

    public static async Task<(DomainResult, User?)> NewAsync(string name,
        IUserValidator _validator,
        CancellationToken cancellationToken)
    {
        var entity = new User();

        var notifications = await _validator.ValidateCreationAsync(entity.FirstName,
            cancellationToken);

        entity.AddNotification(notifications);

        var result = entity.Validate();

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
