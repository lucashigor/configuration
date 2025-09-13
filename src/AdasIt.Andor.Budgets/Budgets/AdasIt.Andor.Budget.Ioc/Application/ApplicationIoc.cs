using AdasIt.Andor.Budget.Application;
using AdasIt.Andor.Budget.Application.Interfaces;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.Currencies;
using AdasIt.Andor.Budgets.Domain.FinancialMovements;
using AdasIt.Andor.Budgets.Domain.Invites;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Domain.Users;
using AdasIt.Andor.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AdasIt.Andor.Budget.Ioc.Application;

internal static class ApplicationIoc
{
    public static IServiceCollection UseApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAkkaModule, ApplicationAkkaModule>();

        services.AddScoped<IAccountValidator, AccountValidator>();
        services.AddScoped<ICategoryValidator, CategoryValidator>();
        services.AddScoped<ICurrencyValidator, CurrencyValidator>();
        services.AddScoped<IFinancialMovementValidator, FinancialMovementValidator>();
        services.AddScoped<IInviteValidator, InviteValidator>();
        services.AddScoped<IPaymentMethodValidator, PaymentMethodValidator>();
        services.AddScoped<ISubCategoryValidator, SubCategoryValidator>();
        services.AddScoped<IUserValidator, UserValidator>();

        services.AddScoped<AccountFactory>();

        services.AddScoped<IAccountQueriesService, AccountQueriesService>();

        services.AddScoped<IAccountCommandsService, AccountCommandsService>();

        return services;
    }
}
