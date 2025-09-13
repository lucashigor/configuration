using AdasIt.Andor.Budgets.Domain.Accounts.Repository;
using AdasIt.Andor.Budgets.InfrastructureCommands;
using AdasIt.Andor.Infrastructure;
using Akka.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc.Infrastructure;

public static class InfrastructureIoc
{
    internal static IServiceCollection UseInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.UseDbContext(configuration);

        services.AddSingleton<IAkkaModule, InfrastructureAkkaModule>();

        services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

        services.AddSingleton<ICommandsAccountRepository>(sp =>
        {
            var supervisor = sp.GetRequiredService<ActorRegistry>().Get<AccountCommandsInfrastructureSupervisor>();
            var eventPublisher = sp.GetRequiredService<IEventPublisher>();
            return new AccountCommandRepository(supervisor, eventPublisher);
        });

        return services;
    }
}