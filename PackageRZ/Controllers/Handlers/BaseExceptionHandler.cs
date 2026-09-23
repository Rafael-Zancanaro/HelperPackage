using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace PackageRZ.Controllers.Handlers;

/// <summary>
/// Provides a base class for implementing strongly-typed exception handlers in ASP.NET Core.
/// </summary>
/// <typeparam name="TException">The specific type of exception to handle.</typeparam>
public abstract class BaseExceptionHandler<TException> : IExceptionHandler where TException : Exception
{
    /// <summary>
    /// Attempts to handle the specified exception asynchronously.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the request.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the exception was handled; otherwise, false.</returns>
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is TException specificException)
        {
            return HandleAsync(httpContext, specificException, cancellationToken);
        }

        return ValueTask.FromResult(false);
    }

    /// <summary>
    /// When overridden in a derived class, handles the specific exception asynchronously.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the request.</param>
    /// <param name="exception">The specific exception to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the exception was handled; otherwise, false.</returns>
    protected abstract ValueTask<bool> HandleAsync(HttpContext httpContext, TException exception, CancellationToken cancellationToken);
}
