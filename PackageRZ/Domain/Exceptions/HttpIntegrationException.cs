using PackageRZ.Domain.Config;
using PackageRZ.Utils;
using System.Net;

namespace PackageRZ.Domain.Exceptions;

/// <summary>
/// Represents a failure in an HTTP call to a dependency.
/// Captures status, method, URL, query, and raw response body at the time it is thrown.
/// </summary>
public class HttpIntegrationException(
    int eventId,
    string url,
    string method,
    int dependencyStatusCode,
    string content,
    string parameters,
    string message,
    HttpStatusCode returnStatus,
    Exception innerException)
    : HelperException(eventId, message, returnStatus, innerException)
{
    /// <summary>
    /// Gets the URL of the HTTP dependency call.
    /// </summary>
    public string Url { get; } = url;

    /// <summary>
    /// Gets the HTTP method used in the call.
    /// </summary>
    public string Method { get; } = method;

    /// <summary>
    /// Gets the HTTP status code returned by the dependency.
    /// </summary>
    public int DependencyStatusCode { get; } = dependencyStatusCode;

    /// <summary>
    /// Gets the raw content of the dependency response.
    /// </summary>
    public string Content { get; } = content;

    /// <summary>
    /// Gets the request parameters.
    /// </summary>
    public string Parameters { get; } = parameters;

    /// <summary>
    /// Creates the exception from the HTTP response, reading the raw body before it is disposed.
    /// </summary>
    /// <param name="responseMessage">The HTTP response message.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    /// <returns>A new instance of <see cref="HttpIntegrationException"/>.</returns>
    public static async Task<HttpIntegrationException> CreateAsync(
        HttpResponseMessage responseMessage,
        Exception innerException = null)
    {
        ArgumentNullException.ThrowIfNull(responseMessage);

        var content = responseMessage.Content is null
            ? null
            : await responseMessage.Content.ReadAsStringAsync();

        var requestMessage = responseMessage.RequestMessage;
        var url = requestMessage?.RequestUri?.ToString();
        var method = requestMessage?.Method?.ToString();
        var statusCode = (int)responseMessage.StatusCode;
        var parameters = await requestMessage.GetParametersAsync();

        return new HttpIntegrationException(
            LogConstants.EventIdUnhandledError,
            url,
            method,
            statusCode,
            content,
            parameters,
            $"HTTP call failed: {method} {url} responded with {statusCode}.",
            HttpStatusCode.InternalServerError,
            innerException);
    }

    /// <summary>
    /// Creates the exception when an HTTP call is rejected by an open circuit breaker.
    /// </summary>
    /// <param name="requestMessage">The HTTP request message.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    /// <returns>A new instance of <see cref="HttpIntegrationException"/>.</returns>
    public static async Task<HttpIntegrationException> CreateOpenCircuitAsync(
        HttpRequestMessage requestMessage,
        Exception innerException)
    {
        ArgumentNullException.ThrowIfNull(requestMessage);
        ArgumentNullException.ThrowIfNull(innerException);

        var url = requestMessage.RequestUri?.ToString();
        var method = requestMessage.Method?.ToString();
        var parameters = await requestMessage.GetParametersAsync();

        return new HttpIntegrationException(
            LogConstants.EventIdUnhandledError,
            url,
            method,
            (int)HttpStatusCode.InternalServerError,
            null,
            parameters,
            $"Circuit breaker open for HTTP call: {method} {url}.",
            HttpStatusCode.InternalServerError,
            innerException);
    }
}
