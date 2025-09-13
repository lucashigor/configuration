namespace AdasIt.Andor.Budgets.InfrastructureCommands.Entities;

internal class SubCategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DeactivationDate { get; set; }
    public Guid CategoryId { get; set; }
    public int Type { get; set; }
    public Guid DefaultPaymentMethodId { get; set; }
}
