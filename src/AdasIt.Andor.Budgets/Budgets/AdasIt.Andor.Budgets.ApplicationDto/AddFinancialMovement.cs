using AdasIt.Andor.ApplicationDto.Commands;

namespace AdasIt.Andor.Budgets.ApplicationDto;

public record AddFinancialMovement(
    Guid Id,
    DateTime Date,
    string Name,
    Guid SubCategoryId,
    int Status,
    Guid PaymentMethodId,
    decimal Value
    ) : ICommands<Guid>
{ }