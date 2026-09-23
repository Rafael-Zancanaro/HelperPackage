using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PackageRZ.Controllers.Filters;

namespace PackageRZ.Utils;

public static class FluentValidationExtensions
{
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
