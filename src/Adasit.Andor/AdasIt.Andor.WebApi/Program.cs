using AdasIt.Andor.Configurations.Ioc;
using AdasIt.Andor.Infrastructure;
using Akka.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.UseConfigurations(builder.Configuration);

builder.Services.AddAkka("MySystem", (builder, provider) =>
{
    var modules = provider.GetServices<IAkkaModule>();
    foreach (var module in modules)
    {
        module.Configure(builder, provider);
    }
});

var app = builder.Build();

app.MapControllers();

app.Run();