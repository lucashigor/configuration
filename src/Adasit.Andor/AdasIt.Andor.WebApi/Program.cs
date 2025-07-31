using AdasIt.Andor.Configurations.Ioc;
using AdasIt.Andor.Infrastructure;
using Akka.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.UseConfigurations(builder.Configuration);

builder.Services.AddAkka("MySystem", (akkaConfigurationBuilder, provider) =>
{
    var modules = provider.GetServices<IAkkaModule>();
    foreach (var module in modules)
    {
        module.Configure(akkaConfigurationBuilder, provider);
    }
});

var app = builder.Build();

app.MapControllers();

app.Run();