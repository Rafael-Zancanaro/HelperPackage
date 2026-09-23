using System.Net;

namespace PackageRZ.Domain.Models;

/// <summary>
/// Represents a failure response with an HTTP status and a list of error messages.
/// </summary>
public readonly struct FailureResult
{
    /// <summary>
    /// Gets the HTTP status code associated with the failure.
    /// </summary>
    public HttpStatusCode Status { get; }

    /// <summary>
    /// Gets the list of error messages.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FailureResult"/> struct.
    /// </summary>
    /// <param name="errors">The list of error messages.</param>
    /// <param name="status">The HTTP status code. Defaults to InternalServerError.</param>
    public FailureResult(IEnumerable<string> errors, HttpStatusCode? status = null)
    {
        Errors = errors.ToList();
        Status = status ?? HttpStatusCode.InternalServerError;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FailureResult"/> struct with a single error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="status">The HTTP status code. Defaults to InternalServerError.</param>
    public FailureResult(string error, HttpStatusCode? status = null)
    {
        Errors = new List<string> { error };
        Status = status ?? HttpStatusCode.InternalServerError;
    }
}
