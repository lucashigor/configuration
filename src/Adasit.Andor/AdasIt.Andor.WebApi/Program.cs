using Adasit.Andor.Auth;
using AdasIt.Andor.ApplicationDto;
using AdasIt.Andor.Configurations.Ioc;
using AdasIt.Andor.Infrastructure;
using AdasIt.Andor.Swagger;
using AdasIt.Andor.WebApi;
using AdasIt.Andor.WebApi.Middlewares;
using Akka.Hosting;
using Andor.Adasit.HealthCheck;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services
    .AddControllers(options =>
    {
        options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
    })
    .AddNewtonsoftJson();

builder.Services.Configure<ApplicationSettings>(builder.Configuration);

builder.Services.UseConfigurations(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
                opts.SerializerOptions.Converters.Add(new ErrorCodeConverter()));
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddAkka("MySystem", (akkaConfigurationBuilder, provider) =>
{
    var modules = provider.GetServices<IAkkaModule>();
    foreach (var module in modules)
    {
        module.Configure(akkaConfigurationBuilder, provider);
    }
});

var app = builder.Build();

app.ConfigureHealthChecks();

var configs = app.Services.GetRequiredService<IOptions<ApplicationSettings>>();

app.UseCustomSwagger(configs.Value.IdentityProvider,
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>());

if (configs.Value?.Cors != null)
{
    app.UseCors(options =>
    {
        options
            .WithOrigins(configs.Value?.Cors?.AllowedOrigins?.ToArray()!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Content-Language");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();