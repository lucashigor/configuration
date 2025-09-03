using AdasIt.Andor.Budgets.Domain.FinancialMovements.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.Validation;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.PaymentMethods;

public class PaymentMethod : Entity<PaymentMethodId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    public MovementType Type { get; private set; } = MovementType.Undefined;
    public int? DefaultOrder { get; private set; }

    public ICollection<SubCategory> SubCategories { get; set; }

    private PaymentMethod()
    {
        Id = PaymentMethodId.New();
        Name = string.Empty;
        Description = string.Empty;
        SubCategories = [];
    }

    private DomainResult SetValues(PaymentMethodId id,
        string name,
        int order)
    {
        AddNotification(name.NotNullOrEmptyOrWhiteSpace());
        AddNotification(name.BetweenLength(3, 70));

        if (Notifications.Count > 1)
        {
            return Validate();
        }

        Name = name;
        DefaultOrder = order;

        var result = Validate();

        return result;
    }

    public static (DomainResult, PaymentMethod?) New(
        string name,
        int order)
    {
        var entity = new PaymentMethod();

        var result = entity.SetValues(PaymentMethodId.New(), name, order);

        if (result.IsFailure)
        {
            return (result, null);
        }

        return (result, entity);
    }
}
