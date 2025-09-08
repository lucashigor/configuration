using AdasIt.Andor.DomainQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.InfrastructureQueries;

public static class DbContextOptionsFactory
{
    public static DbContextOptions<TContext> Create<TContext>(string[] args)
        where TContext : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

        optionsBuilder.UseNpgsql("inmemory");

        return optionsBuilder.Options;
    }
}

public class PrincipalContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<ProcessedEvents> ProcessedEvents => Set<ProcessedEvents>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Name>();
        modelBuilder.Ignore<Description>();
        
        modelBuilder.ApplyConfiguration(new ProcessedEventsConfig());
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.GetForeignKeys()
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<bool>(nameof(ISoftDeletableEntity.IsDeleted))
                    .HasDefaultValue(false);

                entityType.AddSoftDeleteQueryFilter();
            }

            foreach (var property in entityType.GetProperties()
                         .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                var converter = new ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

                property.SetValueConverter(converter);
            }
        }
    }

    public void Upsert<T>(T entity) where T : class
    {
        if (Entry(entity).State == EntityState.Detached)
        {
            Set<T>().Add(entity);
        }
        else if (Entry(entity).State == EntityState.Modified)
        {
            Set<T>().Update(entity);
        }
    }

    public void UpsertRange<T>(IEnumerable<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                Set<T>().Add(entity);
            }
        }
    }
}