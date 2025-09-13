using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts;

namespace AdasIt.Andor.Budget.Application;

internal static class AccountMapperExtensions
{
    public static AccountOutput ToAccountOutput(this Account config)
        => new AccountOutput()
        {
            Id = config.Id,
            Name = config.Name,
            Description = config.Description,
        };
}