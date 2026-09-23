using PackageRZ.Domain.Models;
using PackageRZ.Domain.ViewModels;
using System.Net;

namespace PackageRZ.Services;

/// <summary>
/// Provides a base class for services, offering standardized methods to return successes and failures.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Creates a failure result with a single error message and an optional HTTP status.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>A <see cref="FailureResult"/> struct containing the error.</returns>
    protected static FailureResult AddErrors(string error, HttpStatusCode? status = null)
        => new(error, status);

    /// <summary>
    /// Creates a failure result with multiple error messages and an optional HTTP status.
    /// </summary>
    /// <param name="errors">The collection of error messages.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>A <see cref="FailureResult"/> struct containing the errors.</returns>
    protected static FailureResult AddErrors(IEnumerable<string> errors, HttpStatusCode? status = null)
        => new(errors, status);

    /// <summary>
    /// Creates a successful view model result with the specified data and an optional HTTP status.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="result">The result data.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>A <see cref="ResultViewModel{T}"/> containing the result.</returns>
    protected static ResultViewModel<T> AddResult<T>(T result, HttpStatusCode? status = null)
        => new ResultViewModel<T>().AddResult(result, status);
}
