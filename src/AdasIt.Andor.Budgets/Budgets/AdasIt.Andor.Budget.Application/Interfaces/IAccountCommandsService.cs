using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Budgets.ApplicationDto;

namespace AdasIt.Andor.Budget.Application.Interfaces;

public interface IAccountCommandsService
{
    Task<ApplicationResult<AccountOutput>> CreateAccountAsync(CreateAccount command,
        CancellationToken cancellationToken);
}
