using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackageRZ.Domain.ViewModels;
using PackageRZ.Logger;
using PackageRZ.Utils;
using System.Net;

namespace PackageRZ.Controllers;

/// <summary>
/// A global controller to handle unhandled exceptions across the API.
/// </summary>
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Standard error endpoint called by the ASP.NET Core exception handler middleware.
    /// </summary>
    /// <returns>A standardized failure response model.</returns>
    [Route("error")]
    public async Task<ResultViewModel<string>> Error()
    {
        var result = new ResultViewModel<string>();
        result.AddErrors(HelperResources.InternalError);
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

        _logger.Error(exception.Error, 500, HelperResources.Informations, 
            string.Format(HelperResources.LogInformation, DateTime.Now, HttpContext.User?.Identity?.Name ?? "Unknown"));
        Response.StatusCode = (short)HttpStatusCode.InternalServerError;

        return result;
    }
}