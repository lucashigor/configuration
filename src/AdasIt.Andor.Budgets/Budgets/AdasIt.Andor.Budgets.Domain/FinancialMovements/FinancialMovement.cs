using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.FinancialMovements.ValueObjects;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.PaymentMethods.ValueObjects;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.SubCategories.ValueObjects;
using AdasIt.Andor.Domain.SeedWork;

namespace AdasIt.Andor.Budgets.Domain.FinancialMovements;

public class FinancialMovement : Entity<FinancialMovementId>
{
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }
    public SubCategoryId SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; }
    public MovementType Type { get; private set; }
    public MovementStatus Status { get; private set; }
    public PaymentMethodId PaymentMethodId { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public AccountId AccountId { get; private set; }
    public Account Account { get; private set; }
    public decimal Value { get; private set; }
    public bool IsDeleted { get; private set; }
}
