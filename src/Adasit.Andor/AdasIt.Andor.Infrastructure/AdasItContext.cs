using AdasIt.Andor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace AdasIt.Andor.Infrastructure
{

    public class AdasItContext<TContext> : DbContext where TContext : DbContext
    {
        public AdasItContext(DbContextOptions<TContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

            foreach (var entityType in from entityType in modelBuilder.Model.GetEntityTypes()
                                       where typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType)
                                       select entityType)
            {
                /*
                modelBuilder.Entity(entityType.ClrType)
                                .Property<bool>(nameof(ISoftDeletableEntity.IsDeleted));
                */

                entityType.AddSoftDeleteQueryFilter();
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.Kind == DateTimeKind.Unspecified
                                ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                                : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void Upsert<T>(T entity) where T : class
        {
            if (Entry(entity).State == EntityState.Detached)
                Set<T>().Add(entity);
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
}
