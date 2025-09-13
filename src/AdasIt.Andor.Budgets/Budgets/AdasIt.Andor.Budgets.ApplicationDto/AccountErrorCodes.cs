using AdasIt.Andor.ApplicationDto.Results;

namespace AdasIt.Andor.Budgets.ApplicationDto;

internal sealed record AccountErrorCodes : ApplicationErrorCode
{
    private AccountErrorCodes(int original) : base(original)
    {
    }
    public static readonly AccountErrorCodes NotFound = new(12_000);
}

public record AccountErrors
{
    public static ErrorModel AccountNotFound() => new(AccountErrorCodes.NotFound, "Account Not Found.");
}
