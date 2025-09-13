using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Budgets.Domain.Accounts.Errors;

public class AccountErrorCodes
{
    public static readonly DomainErrorCode ErrorOnCreatingAccount = DomainErrorCode.New(3_001);
}
