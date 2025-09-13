namespace AdasIt.Andor.Budgets.InfrastructureCommands.Entities;

internal class FinancialMovementEntity
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public Guid SubCategoryId { get; set; }
    public SubCategoryEntity SubCategory { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
    public Guid PaymentMethodId { get; set; }
    public PaymentMethodEntity PaymentMethod { get; set; }
    public decimal Value { get; set; }
}
