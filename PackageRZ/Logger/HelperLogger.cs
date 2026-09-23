using Microsoft.Extensions.Logging;
using PackageRZ.Utils;
using static PackageRZ.Domain.Config.Constants;
using static PackageRZ.Domain.Config.LogConstants;

namespace PackageRZ.Logger;

/// <summary>
/// A centralized structured logging class for PackageRZ.
/// Utilizes LoggerMessage source generators for zero-allocation logging when the log level is disabled.
/// </summary>
public static partial class HelperLogger
{
    /// <summary>
    /// Logs an error message using the standard template.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="message">The main log message.</param>
    /// <param name="details">Additional details.</param>
    /// <param name="origin">The origin of the log.</param>
    public static void Error(this ILogger logger, int eventId, string message = NotSpecified, string details = NotSpecified, string origin = NotSpecified)
        => LogError(logger, eventId, origin ?? NotSpecified, message, details);

    /// <summary>
    /// Logs an error message with an associated exception using the standard template.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="message">The main log message.</param>
    /// <param name="details">Additional details.</param>
    /// <param name="origin">The origin of the log.</param>
    public static void Error(this ILogger logger, Exception exception, int eventId, string message = NotSpecified, string details = NotSpecified, string origin = NotSpecified)
        => LogErrorException(logger, exception, eventId, origin ?? NotSpecified, message, details);

    /// <summary>
    /// Logs a warning message using the standard template.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="message">The main log message.</param>
    /// <param name="details">Additional details.</param>
    /// <param name="origin">The origin of the log.</param>
    public static void Warning(this ILogger logger, int eventId, string message = NotSpecified, string details = NotSpecified, string origin = NotSpecified)
        => LogWarning(logger, eventId, origin ?? NotSpecified, message, details);

    /// <summary>
    /// Asynchronously logs an HTTP error based on the HTTP response message.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="responseMessage">The HTTP response message.</param>
    /// <param name="origin">The origin of the log.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task HttpErrorAsync(this ILogger logger, int eventId, HttpResponseMessage responseMessage, string origin = NotSpecified)
    {
        LogHttpError(
            logger,
            eventId,
            origin ?? NotSpecified,
            (int)responseMessage.StatusCode,
            responseMessage.RequestMessage?.Method.ToString() ?? NotSpecified,
            responseMessage.RequestMessage?.RequestUri?.ToString() ?? NotSpecified,
            await responseMessage.RequestMessage.GetParametersAsync() ?? NotSpecified,
            await responseMessage.Content.ReadAsStringAsync()
        );
    }

    /// <summary>
    /// Logs an HTTP error with an associated exception.
    /// </summary>
    public static void HttpError(this ILogger logger, Exception exception, int eventId, int statusCode, string method, string url, string content, string parameters = NotSpecified, string origin = NotSpecified)
        => LogHttpErrorException(
            logger,
            exception,
            eventId,
            origin ?? NotSpecified,
            statusCode,
            method ?? NotSpecified,
            url ?? NotSpecified,
            parameters ?? NotSpecified,
            content ?? NotSpecified);

    /// <summary>
    /// Logs an HTTP error with an associated exception, omitting the response content.
    /// </summary>
    public static void HttpError(this ILogger logger, Exception exception, int eventId, int statusCode, string method, string url, string parameters = NotSpecified, string origin = NotSpecified)
        => LogHttpErrorExceptionWithoutContent(
            logger,
            exception,
            eventId,
            origin ?? NotSpecified,
            statusCode,
            method ?? NotSpecified,
            url ?? NotSpecified,
            parameters ?? NotSpecified);

    /// <summary>
    /// Asynchronously logs an HTTP warning based on the HTTP response message.
    /// </summary>
    public static async Task HttpWarningAsync(this ILogger logger, int eventId, HttpResponseMessage responseMessage, string origin = NotSpecified)
    {
        LogHttpWarning(
            logger,
            eventId,
            origin ?? NotSpecified,
            (int)responseMessage.StatusCode,
            responseMessage.RequestMessage?.Method.ToString() ?? NotSpecified,
            responseMessage.RequestMessage?.RequestUri?.ToString() ?? NotSpecified,
            await responseMessage.RequestMessage.GetParametersAsync() ?? NotSpecified,
            await responseMessage.Content.ReadAsStringAsync()
        );
    }

    [LoggerMessage(Level = LogLevel.Error, Message = MessageTemplate)]
    private static partial void LogError(ILogger logger, int eventId, string origin, string message, string details);

    [LoggerMessage(Level = LogLevel.Error, Message = MessageTemplate)]
    private static partial void LogErrorException(ILogger logger, Exception exception, int eventId, string origin, string message, string details);

    [LoggerMessage(Level = LogLevel.Warning, Message = MessageTemplate)]
    private static partial void LogWarning(ILogger logger, int eventId, string origin, string message, string details);

    [LoggerMessage(Level = LogLevel.Error, Message = HttpTemplate)]
    private static partial void LogHttpError(ILogger logger, int eventId, string origin, int statusCode, string method, string url, string parameters, string content);

    [LoggerMessage(Level = LogLevel.Error, Message = HttpTemplate)]
    private static partial void LogHttpErrorException(ILogger logger, Exception exception, int eventId, string origin, int statusCode, string method, string url, string parameters, string content);

    [LoggerMessage(Level = LogLevel.Error, Message = HttpTemplateWithoutContent)]
    private static partial void LogHttpErrorExceptionWithoutContent(ILogger logger, Exception exception, int eventId, string origin, int statusCode, string method, string url, string parameters);

    [LoggerMessage(Level = LogLevel.Warning, Message = HttpTemplate)]
    private static partial void LogHttpWarning(ILogger logger, int eventId, string origin, int statusCode, string method, string url, string parameters, string content);
}
