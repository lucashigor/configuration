using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Budget.Application.Actors;
using AdasIt.Andor.Budget.Application.Interfaces;
using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Domain.ValuesObjects;
using Akka.Actor;
using Akka.Hosting;

namespace AdasIt.Andor.Budget.Application;

public class AccountCommandsService(ActorRegistry registry) : IAccountCommandsService
{
    private readonly IActorRef _configActor = registry.Get<AccountManagerActor>();

    public async Task<ApplicationResult<AccountOutput>> CreateAccountAsync(CreateAccount input,
        CancellationToken cancellationToken)
    {
        return await Handler(input, cancellationToken);
    }

    private async Task<ApplicationResult<AccountOutput>> Handler(object command, CancellationToken cancellationToken)
    {
        var response = ApplicationResult<AccountOutput>.Success();

        var (result, account) = await _configActor.Ask<(DomainResult, Account)>(command,
            cancellationToken);

        if (result.IsFailure)
        {
            await HandleAccountResult.HandleResultAccount(result, response);
            return response;
        }

        response.SetData(account.ToAccountOutput());

        return response;
    }
}
