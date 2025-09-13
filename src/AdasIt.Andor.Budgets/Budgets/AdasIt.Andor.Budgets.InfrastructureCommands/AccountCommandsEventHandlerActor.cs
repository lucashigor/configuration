using Adasit.Andor.Mapping;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.Commands;
using AdasIt.Andor.Budgets.Domain.Accounts.Events;
using AdasIt.Andor.Budgets.InfrastructureCommands.Entities;
using Akka.Actor;
using Akka.Persistence;

namespace AdasIt.Andor.Budgets.InfrastructureCommands;

public class AccountCommandsEventHandlerActor : ReceivePersistentActor
{
    private readonly Guid _accountId;
    private AccountEntity _account;

    public override string PersistenceId => $"account-{_accountId}";

    public AccountCommandsEventHandlerActor(Guid accountId)
    {
        _accountId = accountId;

        Command<LoadAccount>(HandleLoadAccount);
        Command<AccountCreated>(HandleAccountCreated);

        Recover<AccountCreated>(Apply);
    }

    private void HandleLoadAccount(LoadAccount evt)
    {
        if (_account == null)
        {
            Context.Sender.Tell(null);

            return;
        }

        var configuration = Mappings.GetValid<Account, AccountEntity>(_account);

        Context.Sender.Tell(configuration);
    }

    private void HandleAccountCreated(AccountCreated evt)
    {
        if (_account != null)
        {
            Sender.Tell(new Status.Failure(new InvalidOperationException("Configuration already exists.")));
            return;
        }

        Persist(evt, e =>
        {
            Apply(e);
            Sender.Tell(new Status.Success($"Account {_account} created."));
        });
    }

    private void Apply(AccountCreated evt)
    {
        if (_account == null)
        {
            _account = new AccountEntity();
        }

        _account.Name = evt.Name;
        _account.Description = evt.Description;
        _account.Currency = new CurrencyEntity() { Id = evt.CurrencyId };
        _account.Status = evt.StatusId;
    }
}
