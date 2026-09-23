using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PackageRZ.DelegateHandlers;
using Polly;
using Polly.Extensions.Http;

namespace PackageRZ.Utils;

/// <summary>
/// Provides extension methods for configuring Polly policies on HTTP clients.
/// </summary>
public static class PolicyExtensions
{
    /// <summary>
    /// Gets a standard retry policy that handles transient HTTP errors and retries 
    /// with an exponential backoff.
    /// </summary>
    /// <param name="retryCount">The maximum number of retries. Defaults to 3.</param>
    /// <returns>An asynchronous policy for HTTP response messages.</returns>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount = 3) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    /// <summary>
    /// Registers the <see cref="HttpCircuitBreakerDelegatingHandler"/> and applies the specified circuit breaker policy.
    /// This extension should be used instead of standard AddPolicyHandler to ensure circuit breaker exceptions 
    /// are properly caught and mapped to <see cref="Domain.Exceptions.HttpIntegrationException"/>.
    /// </summary>
    /// <param name="httpClientBuilder">The HTTP client builder.</param>
    /// <param name="circuitBreakerPolicy">The circuit breaker policy to apply.</param>
    /// <returns>The HTTP client builder.</returns>
    public static IHttpClientBuilder AddHelperPolicyHandler(
        this IHttpClientBuilder httpClientBuilder,
        IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy)
    {
        httpClientBuilder.Services.TryAddTransient<HttpCircuitBreakerDelegatingHandler>();

        return httpClientBuilder
            .AddHttpMessageHandler<HttpCircuitBreakerDelegatingHandler>()
            .AddPolicyHandler(circuitBreakerPolicy);
    }

    /// <summary>
    /// Gets a standard circuit breaker policy that handles transient HTTP errors.
    /// </summary>
    /// <param name="handledEventsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit. Defaults to 3.</param>
    /// <param name="durationOfBreakSeconds">The duration the circuit will stay open. Defaults to 30 seconds.</param>
    /// <returns>An asynchronous policy for HTTP response messages.</returns>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
        int handledEventsAllowedBeforeBreaking = 3,
        TimeSpan? durationOfBreakSeconds = null)
    {
        durationOfBreakSeconds ??= TimeSpan.FromSeconds(30);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreakSeconds.Value);
    }
}
