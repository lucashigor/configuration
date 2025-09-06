using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public class PaymentMethod : Entity<PaymentMethodId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public MovementType Type { get; private set; }

    private PaymentMethod()
    {
        Name = string.Empty;
        Description = string.Empty;
        StartDate = DateTime.UtcNow;
        Type = MovementType.Undefined;
    }

    public PaymentMethod(string name, string description, DateTime startDate,
        DateTime? deactivationDate, MovementType type)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DeactivationDate = deactivationDate;
        Type = type;
    }

    public static async Task<(DomainResult, PaymentMethod?)> NewAsync(string name, string description,
        DateTime startDate, DateTime? deactivationDate, MovementType type,
        IPaymentMethodValidator _validator,
        CancellationToken cancellationToken)
    {
        var entity = new PaymentMethod();

        var notifications = await _validator.ValidateCreationAsync(entity.Name,
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
