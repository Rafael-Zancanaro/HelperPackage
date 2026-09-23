using Microsoft.AspNetCore.Http;
using PackageRZ.Domain.ViewModels;
using System.Net;

namespace PackageRZ.Controllers.Handlers;

/// <summary>
/// Provides extension methods for HTTP responses to write standardized error view models.
/// </summary>
public static class ErrorResponseExtensions
{
    /// <summary>
    /// Writes a standardized error response asynchronously to the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="status">The HTTP status code to return. Defaults to InternalServerError.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async ValueTask<bool> WriteStandardErrorAsync(
        this HttpContext httpContext,
        HttpStatusCode status = HttpStatusCode.InternalServerError,
        CancellationToken cancellationToken = default)
    {
        httpContext.Response.StatusCode = (int)status;
        var result = CreateStandardError();
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }

    /// <summary>
    /// Creates a standard error view model with a generic internal error message.
    /// </summary>
    /// <returns>A <see cref="ResultViewModel{T}"/> configured for a failure state.</returns>
    internal static ResultViewModel<bool> CreateStandardError()
        => new ResultViewModel<bool>().AddErrors(PackageRZ.Utils.HelperResources.InternalError);
}
