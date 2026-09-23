using PackageRZ.Domain.Models;
using System.Net;
using System.Text.Json.Serialization;

namespace PackageRZ.Domain.ViewModels;

/// <summary>
/// Represents a generic view model wrapper that standardizes API responses.
/// </summary>
/// <typeparam name="T">The type of the result data.</typeparam>
public class ResultViewModel<T>
{
    /// <summary>
    /// Gets or sets the optional HTTP return status associated with the result.
    /// This property is ignored during JSON serialization.
    /// </summary>
    [JsonIgnore]
    public HttpStatusCode? ReturnStatus { get; set; }

    /// <summary>
    /// Gets or sets the data returned by the operation.
    /// </summary>
    public T Result { get; set; }

    private List<string> _errors;

    /// <summary>
    /// Gets the collection of error messages, if any.
    /// </summary>
    public IReadOnlyCollection<string> Errors
        => _errors is not null ? _errors : Array.Empty<string>();

    /// <summary>
    /// Gets a value indicating whether the operation was successful (i.e., no errors).
    /// </summary>
    public bool Success => _errors is null || _errors.Count == 0;

    /// <summary>
    /// Defines an implicit conversion from a <see cref="FailureResult"/> to a <see cref="ResultViewModel{T}"/>.
    /// </summary>
    /// <param name="failure">The failure result to convert.</param>
    public static implicit operator ResultViewModel<T>(FailureResult failure)
        => new ResultViewModel<T>().AddErrors(failure.Errors, failure.Status);

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultViewModel{T}"/> class.
    /// </summary>
    public ResultViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultViewModel{T}"/> class with the specified result and errors.
    /// </summary>
    /// <param name="result">The result data.</param>
    /// <param name="errors">The collection of error messages.</param>
    [JsonConstructor]
    public ResultViewModel(T result, IReadOnlyCollection<string> errors)
    {
        Result = result;

        if (errors is { Count: > 0 })
            _errors = errors as List<string> ?? errors.ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultViewModel{T}"/> class with the specified result.
    /// </summary>
    /// <param name="result">The result data.</param>
    public ResultViewModel(T result)
    {
        Result = result;
    }

    /// <summary>
    /// Adds a single error message to the view model.
    /// </summary>
    /// <param name="error">The error message to add.</param>
    /// <returns>The current instance of <see cref="ResultViewModel{T}"/>.</returns>
    public ResultViewModel<T> AddError(string error)
        => AddErrors(error);

    /// <summary>
    /// Adds a single error message and an optional HTTP status code to the view model.
    /// </summary>
    /// <param name="error">The error message to add.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>The current instance of <see cref="ResultViewModel{T}"/>.</returns>
    public ResultViewModel<T> AddErrors(string error, HttpStatusCode? status = null)
    {
        _errors ??= new List<string>(1);
        _errors.Add(error);

        if (status is not null)
            ReturnStatus = status;

        return this;
    }

    /// <summary>
    /// Adds a collection of error messages and an optional HTTP status code to the view model.
    /// </summary>
    /// <param name="errors">The collection of error messages to add.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>The current instance of <see cref="ResultViewModel{T}"/>.</returns>
    public ResultViewModel<T> AddErrors(IEnumerable<string> errors, HttpStatusCode? status = null)
    {
        _errors ??= new List<string>();
        _errors.AddRange(errors);

        if (status is not null)
            ReturnStatus = status;

        return this;
    }

    /// <summary>
    /// Sets the result data and an optional HTTP status code.
    /// </summary>
    /// <param name="value">The result data.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <returns>The current instance of <see cref="ResultViewModel{T}"/>.</returns>
    public ResultViewModel<T> AddResult(T value, HttpStatusCode? status = null)
    {
        Result = value;

        if (status is not null)
            ReturnStatus = status;

        return this;
    }
}