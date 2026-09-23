using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PackageRZ.Domain.ViewModels;
using PackageRZ.Utils;
using System.Collections.Concurrent;

namespace PackageRZ.Controllers.Filters;

/// <summary>
/// An asynchronous action filter that validates action arguments using FluentValidation.
/// Short-circuits the request with a BadRequest and standardized error response if validation fails.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    private static readonly ConcurrentDictionary<Type, Type> _validatorTypeCache = new();

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// Evaluates all complex type arguments against registered FluentValidation validators.
    /// </summary>
    /// <param name="context">The context for the action execution.</param>
    /// <param name="next">The delegate to execute the next filter or the action itself.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor.Parameters.Count == 0)
        {
            await next();
            return;
        }

        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            var argumentType = parameter.ParameterType;

            if (argumentType.IsValueType || argumentType == typeof(string) || argumentType == typeof(CancellationToken))
                continue;

            var validatorType = _validatorTypeCache.GetOrAdd(
                argumentType,
                t => typeof(IValidator<>).MakeGenericType(t));

            var requestServices = context.HttpContext.RequestServices;
            if (requestServices.GetService(validatorType) is not IValidator validator)
                continue;

            if (!context.ActionArguments.TryGetValue(parameter.Name, out var argumentValue) || argumentValue is null)
            {
                var result = new ResultViewModel<string>().AddErrors(HelperResources.InternalError);
                context.Result = new BadRequestObjectResult(result);
                return;
            }

            var validationContext = new ValidationContext<object>(argumentValue);
            var validationResult = await validator.ValidateAsync(validationContext);

            if (!validationResult.IsValid)
            {
                var messages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var result = new ResultViewModel<string>().AddErrors(messages);
                context.Result = new BadRequestObjectResult(result);
                return;
            }
        }

        await next();
    }
}
