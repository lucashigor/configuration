using Akka.Hosting;

namespace AdasIt.Andor.Infrastructure;

public interface IAkkaModule
{
    void Configure(AkkaConfigurationBuilder builder, IServiceProvider provider);
}
