using AdasIt.Andor.Configurations.Infrastructure.Config;
using AdasIt.Andor.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdasIt.Andor.Configurations.Infrastructure.Repositories;

public class ConfigurationFactory : IDesignTimeDbContextFactory<ConfigurationContext>
{
    public ConfigurationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConfigurationContext>();
        optionsBuilder.UseNpgsql("inmemory");

        return new ConfigurationContext(optionsBuilder.Options);
    }
}

public class ConfigurationContext(DbContextOptions<ConfigurationContext> options) : AdasItContext<ConfigurationContext>(options)
{
    public DbSet<ConfigurationDto> Configuration => Set<ConfigurationDto>();
}

