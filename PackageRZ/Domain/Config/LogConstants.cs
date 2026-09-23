namespace PackageRZ.Domain.Config;

/// <summary>
/// Provides global constant values for logging templates.
/// </summary>
public static class LogConstants
{
    /// <summary>
    /// Event ID for unhandled errors.
    /// </summary>
    public const int EventIdUnhandledError = 0;

    /// <summary>
    /// Log template for an unhandled error.
    /// </summary>
    public const string UnhandledError = "Unhandled error: {0} - {1}";

    /// <summary>
    /// Log template for request details.
    /// </summary>
    public const string RequestDetails = "Route: {0} [{1}] | PersonId: {2}";

    /// <summary>
    /// Log template for a system error.
    /// </summary>
    public const string SystemError = "A system error occurred. Date: {ErrorDate}";

    /// <summary>
    /// The standard message template used by the logger.
    /// </summary>
    public const string MessageTemplate = "[{EventId}] | Origin: [{Origin}] | Message: {Message} | Details: {Details}";

    /// <summary>
    /// The standard HTTP log template used by the logger, including the response content.
    /// </summary>
    public const string HttpTemplate = "[{EventId}] | Origin: [{Origin}] | Status: {StatusCode} | Method: {Method} | Url: {Url} | Parameters: {Parameters} | Response: {Content}";

    /// <summary>
    /// The standard HTTP log template used by the logger, omitting the response content.
    /// </summary>
    public const string HttpTemplateWithoutContent = "[{EventId}] | Origin: [{Origin}] | Status: {StatusCode} | Method: {Method} | Url: {Url} | Parameters: {Parameters}";
}
