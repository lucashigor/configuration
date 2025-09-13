using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public class PaymentMethod : Entity<PaymentMethodId>
{
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public MovementType Type { get; private set; }

    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected PaymentMethod()
#pragma warning restore CS8618, CS9264
    {
    }

    private PaymentMethod(Name name, Description description, DateTime startDate,
        DateTime? deactivationDate, MovementType type)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DeactivationDate = deactivationDate;
        Type = type;
    }

    public static async Task<(DomainResult, PaymentMethod?)> NewAsync(Name name, Description description,
        DateTime startDate, DateTime? deactivationDate, MovementType type, IPaymentMethodValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new PaymentMethod(name, description, startDate, deactivationDate, type);

        return await entity.ValidateAsync(validator, cancellationToken);
    }
}
