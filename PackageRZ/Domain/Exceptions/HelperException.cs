using System.Net;

namespace PackageRZ.Domain.Exceptions;

/// <summary>
/// Represents the base exception class for the Helper package.
/// </summary>
/// <param name="eventId">The event identifier associated with the error.</param>
/// <param name="message">The error message.</param>
/// <param name="returnStatus">The HTTP status code to return. Defaults to InternalServerError.</param>
/// <param name="innerException">The inner exception that caused this exception.</param>
public abstract class HelperException(
    int eventId,
    string message,
    HttpStatusCode returnStatus = HttpStatusCode.InternalServerError,
    Exception innerException = null) : Exception(message, innerException)
{
    /// <summary>
    /// Gets the event identifier associated with the error.
    /// </summary>
    public int EventId { get; } = eventId;

    /// <summary>
    /// Gets the HTTP status code to return.
    /// </summary>
    public HttpStatusCode ReturnStatus { get; } = returnStatus;
}
