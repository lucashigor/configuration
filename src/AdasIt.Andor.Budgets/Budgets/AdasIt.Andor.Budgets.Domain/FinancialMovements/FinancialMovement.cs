using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.FinancialMovements;

public class FinancialMovement : Entity<FinancialMovementId>
{
    public DateTime Date { get; private set; }
    public Name? Name { get; private set; }
    public SubCategoryId SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; }
    public MovementType Type { get; private set; }
    public MovementStatus Status { get; private set; }
    public PaymentMethodId PaymentMethodId { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public decimal Value { get; private set; }
    
    /// <summary>
    /// Used to be constructed via reflection in: EventSourcing repository, ORM, etc.
    /// </summary>
#pragma warning disable CS8618, CS9264
    protected FinancialMovement()
#pragma warning restore CS8618, CS9264
    {
    }
    
    private FinancialMovement(DateTime date, Name? name, SubCategory subCategory,
        MovementStatus status, PaymentMethod paymentMethod, decimal value)
    {
        Date = date;
        Name = name;
        SubCategory = subCategory;
        SubCategoryId = subCategory.Id;
        Type = subCategory.Type;
        Status = status;
        PaymentMethod = paymentMethod;
        PaymentMethodId = paymentMethod.Id;
        Value = value;
    }

    public static async Task<(DomainResult, FinancialMovement?)> NewAsync(DateTime date, Name? name, 
        SubCategory subCategory, MovementStatus status, PaymentMethod paymentMethod, decimal value,
        IFinancialMovementValidator validator,
        CancellationToken cancellationToken)
    {
        var entity = new FinancialMovement(date, name, subCategory, status, paymentMethod, value);
        
        var result =  await entity.ValidateAsync(validator, cancellationToken);

        return result;
    }
}