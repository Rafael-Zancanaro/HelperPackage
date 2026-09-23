using PackageRZ.Domain.Exceptions;
using Polly.CircuitBreaker;

namespace PackageRZ.DelegateHandlers;

/// <summary>
/// Converts the Polly circuit breaker exception into a standardized HTTP integration exception,
/// preserving the data of the request that was blocked.
/// </summary>
public sealed class HttpCircuitBreakerDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
    /// Intercepts <see cref="BrokenCircuitException"/> to throw a standardized <see cref="HttpIntegrationException"/>.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (BrokenCircuitException exception)
        {
            throw await HttpIntegrationException.CreateOpenCircuitAsync(request, exception);
        }
    }
}
