using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PackageRZ.Controllers.Filters;

namespace PackageRZ.Utils;

/// <summary>
/// Provides extension methods for setting up FluentValidation with ASP.NET Core MVC.
/// </summary>
public static class FluentValidationExtensions
{
    /// <summary>
    /// Configures FluentValidation to work seamlessly with controllers, registering all validators in the assembly of the specified type.
    /// </summary>
    /// <typeparam name="T">A type within the assembly containing the validators to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="stopOnFirstFailure">If true, validation will stop on the first failure globally.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddFluentValidation<T>(this IServiceCollection services, bool stopOnFirstFailure = false)
    {
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        services.AddValidatorsFromAssemblyContaining<T>();

        services.AddControllers(options =>
        {
            options.ModelValidatorProviders.Clear();
            options.Filters.Add<ValidationFilter>();
        });

        if (stopOnFirstFailure)
        {
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        }

        return services;
    }
}
