using AdasIt.Andor.Configurations.Domain.Repository;
using AdasIt.Andor.Configurations.DomainQueries;
using AdasIt.Andor.Configurations.InfrastructureCommands;
using AdasIt.Andor.Configurations.InfrastructureQueries;
using AdasIt.Andor.Infrastructure;
using Akka.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.Ioc.Infrastructure;

internal static class InfrastructureIoc
{
    internal static IServiceCollection UseInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.UseDbContext(configuration);

        services.AddSingleton<IAkkaModule, InfrastructureAkkaModule>();

        services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

        services.AddScoped<IQueriesConfigurationRepository,
            QueriesConfigurationRepository>();

        services.AddSingleton<ICommandsConfigurationRepository>(sp =>
        {
            var supervisor = sp.GetRequiredService<ActorRegistry>().Get<ConfigurationCommandsInfrastructureSupervisor>();
            var eventPublisher = sp.GetRequiredService<IEventPublisher>();
            return new ConfigurationCommandRepository(supervisor, eventPublisher);
        });

        return services;
    }
}
