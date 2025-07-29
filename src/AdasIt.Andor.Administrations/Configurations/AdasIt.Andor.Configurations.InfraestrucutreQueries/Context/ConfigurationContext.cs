using AdasIt.Andor.DomainQueries;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace AdasIt.Andor.Configurations.InfrastructureQueries.Context;
public class ConfigurationContextFactory : IDesignTimeDbContextFactory<ConfigurationContext>
{
    public ConfigurationContext CreateDbContext(string[] args)
    {
        var options = DbContextOptionsFactory.Create<ConfigurationContext>(args);
        return new ConfigurationContext(options);
    }
}
public class ConfigurationContext
    : PrincipalContext
{
    public ConfigurationContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<ConfigurationOutput> Configuration => Set<ConfigurationOutput>();
    public DbSet<ProcessedEvents> ProcessedEvents => Set<ProcessedEvents>();

}
