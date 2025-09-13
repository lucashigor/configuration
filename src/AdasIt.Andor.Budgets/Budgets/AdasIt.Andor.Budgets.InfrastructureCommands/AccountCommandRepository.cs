using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.Commands;
using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Infrastructure;
using Akka.Actor;

namespace AdasIt.Andor.Budgets.InfrastructureCommands;

public class AccountCommandRepository : ICommandsAccountRepository
{
    private readonly IActorRef _supervisor;
    private readonly IEventPublisher _eventPublisher;

    public AccountCommandRepository(IActorRef supervisor, IEventPublisher eventPublisher)
    {
        _supervisor = supervisor;
        _eventPublisher = eventPublisher;
    }

    public async Task<Account> GetByIdAsync(AccountId id, CancellationToken cancellationToken)
    {
        var result = await _supervisor.Ask<Account?>(new LoadAccount(id),
            cancellationToken: cancellationToken);

        return result;
    }

    public Task<Currency> GetDefaultCurrency(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task PersistAsync(Account entity, CancellationToken cancellationToken)
    {
        var publishTasks = entity.Events
            .Select(@event => _eventPublisher.PublishAsync(@event, cancellationToken))
            .ToArray();

        await Task.WhenAll(publishTasks);

        entity.ClearEvents();
    }
}
