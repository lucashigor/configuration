using System.Reflection;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.FinancialMovements;
using AdasIt.Andor.Budgets.Domain.Invites;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.InfrastructureQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdasIt.Andor.Budgets.InfrastructureQueries.Context;

public class BudgetContextFactory : IDesignTimeDbContextFactory<BudgetContext>
{
    public BudgetContext CreateDbContext(string[] args)
    {
        var options = DbContextOptionsFactory.Create<BudgetContext>(args);
        return new BudgetContext(options);
    }
}

public class BudgetContext
    : PrincipalContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Account> Account => Set<Account>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Currency> Currency => Set<Currency>();
    public DbSet<FinancialMovement> FinancialMovement => Set<FinancialMovement>();
    public DbSet<Invite> Invite => Set<Invite>();
    public DbSet<PaymentMethod> PaymentMethod => Set<PaymentMethod>();
    public DbSet<SubCategory> SubCategory => Set<SubCategory>();
    public DbSet<User> User => Set<User>();
}