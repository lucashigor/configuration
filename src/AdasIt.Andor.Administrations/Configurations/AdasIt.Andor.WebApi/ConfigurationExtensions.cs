﻿using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Configurations.WebApi;

public static class ApiExtensions
{
    public static IServiceCollection UseApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ConfigurationController).Assembly));

        return services;
    }
}
