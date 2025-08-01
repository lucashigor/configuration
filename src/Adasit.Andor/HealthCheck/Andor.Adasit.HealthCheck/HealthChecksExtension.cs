using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Andor.Adasit.HealthCheck
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationContext = configuration.GetConnectionString("ConfigurationContext");

            if (configurationContext is not null)
            {
                services.AddHealthChecks();

                services.AddHealthChecks()
                    .AddNpgSql(configurationContext, healthQuery: "select 1", name: "ConfigurationContext", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Database" });
            }

            var Akka_Persistence = configuration["Akka:Persistence:ConnectionString"];

            if (Akka_Persistence is not null)
            {
                services.AddHealthChecks();

                services.AddHealthChecks()
                    .AddNpgSql(Akka_Persistence, healthQuery: "select 1", name: "Akka_Persistence", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Database" });
            }

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10);
                opt.MaximumHistoryEntriesPerEndpoint(60);
                opt.SetApiMaxActiveRequests(1);
                opt.AddHealthCheckEndpoint("Andor api", "/health");

            })
                .AddInMemoryStorage();
            return services;
        }

        public static WebApplication ConfigureHealthChecks(this WebApplication app)
        {

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).WithMetadata(new AllowAnonymousAttribute());

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/healthcheck-ui";
            });

            return app;
        }

    }
}
