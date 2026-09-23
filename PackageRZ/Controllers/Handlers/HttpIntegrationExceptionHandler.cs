using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PackageRZ.Domain.Exceptions;
using PackageRZ.Logger;

namespace PackageRZ.Controllers.Handlers;

/// <summary>
/// Handles HTTP call failures to dependencies.
/// Logs status, method, URL, and parameters without logging the response payload.
/// </summary>
public class HttpIntegrationExceptionHandler(ILogger<HttpIntegrationExceptionHandler> logger)
    : BaseExceptionHandler<HttpIntegrationException>
{
    /// <summary>
    /// Handles the specific HTTP integration exception asynchronously.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the request.</param>
    /// <param name="exception">The HTTP integration exception.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override ValueTask<bool> HandleAsync(HttpContext httpContext, HttpIntegrationException exception, CancellationToken cancellationToken)
    {
        // Content omitted from log writing according to business rules
        logger.HttpError(
            exception,
            exception.EventId,
            exception.DependencyStatusCode,
            exception.Method,
            exception.Url,
            parameters: exception.Parameters);

        return httpContext.WriteStandardErrorAsync(exception.ReturnStatus, cancellationToken);
    }
}
