using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PackageRZ.Domain.ViewModels;
using System.Collections.Concurrent;

namespace PackageRZ.Controllers.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private static readonly ConcurrentDictionary<Type, Type> _validatorTypeCache = new();

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
                var result = new ResultViewModel<string>().AddErros(PackageRZ.Utils.HelperResources.InternalError);
                context.Result = new BadRequestObjectResult(result);
                return;
            }

            var validationContext = new ValidationContext<object>(argumentValue);
            var validationResult = await validator.ValidateAsync(validationContext);

            if (!validationResult.IsValid)
            {
                var messages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var result = new ResultViewModel<string>().AddErros(messages);
                context.Result = new BadRequestObjectResult(result);
                return;
            }
        }

        await next();
    }
}
