using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.SubCategories;

public interface ISubCategoryValidator
{
    Task<List<Notification>> ValidateCreationAsync(string name,
        string Description,
        DateTime StartDate,
        DateTime DeactivationDate,
        Category Category,
        PaymentMethod DefaultPaymentMethod,
        CancellationToken cancellationToken);
}
