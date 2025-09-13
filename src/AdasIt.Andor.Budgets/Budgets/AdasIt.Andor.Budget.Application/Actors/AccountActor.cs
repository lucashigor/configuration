using AdasIt.Andor.Budgets.ApplicationDto;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Budgets.Domain.Accounts.ValueObjects;
using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Application.Actors;

public class AccountActor : ReceiveActor, IWithUnboundedStash
{
    private readonly AccountId _id;
    private Account? _account;
    private readonly IServiceProvider _serviceProvider;

    public IStash? Stash { get; set; }

    public AccountActor(AccountId id,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _id = id;

        Become(Loading);
    }

    protected override void PreStart()
    {
        Self.Tell(new PreLoadConfiguration(_id));

        base.PreStart();
    }

    private void Loading()
    {
        ReceiveAsync<PreLoadConfiguration>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandsAccountRepository>();

            var result = await commandRepository.GetByIdAsync(_id, CancellationToken.None);

            if (result == null) return;

            _account = result;

            Become(Ready);
            ProcessStash();
        });

        ReceiveAsync<CreateAccount>(async cmd =>
        {
            using var scope = _serviceProvider.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<AccountFactory>();
            var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandsAccountRepository>();

            var (result, config) = await factory.CreateAsync(
                cmd.Name,
                cmd.Description,
                cmd.CancellationToken);

            if (config != null && config.Events.Count != 0)
            {
                await commandRepository.PersistAsync(config, CancellationToken.None);
            }

            _account = config;

            Sender.Tell((result, config));
        });

        Receive<AddFinancialMovement>(msg => Stash!.Stash());
    }

    private void Ready()
    {
        ReceiveAsync<AddFinancialMovement>(async cmd =>
        {
        });
    }

    private void ProcessStash()
    {
        Stash!.UnstashAll();
    }

    private record PreLoadConfiguration(AccountId Id);
}