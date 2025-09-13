using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts.Errors;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budget.Application;

public static class HandleAccountResult
{
    private static readonly Dictionary<DomainErrorCode, ErrorModel> _errorsMapping = new()
        {
            { AccountErrorCodes.ErrorOnCreatingAccount, AccountErrors.AccountNotFound() }
        };

    public static async Task HandleResultAccount<T>(DomainResult result,
        ApplicationResult<T> notifier) where T : class
    {
        await HandleResult.Handle(result, notifier, _errorsMapping);
    }
}
