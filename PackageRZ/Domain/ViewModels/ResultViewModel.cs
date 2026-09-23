using PackageRZ.Domain.Models;
using System.Net;
using System.Text.Json.Serialization;

namespace PackageRZ.Domain.ViewModels;

public class ResultViewModel<T>
{
    [JsonIgnore]
    public HttpStatusCode? StatusRetorno { get; set; }

    public T Result { get; set; }

    private List<string> _errors;

    public IReadOnlyCollection<string> Errors
        => _errors is not null ? _errors : Array.Empty<string>();

    public bool Success => _errors is null || _errors.Count == 0;

    public static implicit operator ResultViewModel<T>(ResultFalha failure)
        => new ResultViewModel<T>().AddErros(failure.Errors, failure.Status);

    public ResultViewModel() { }

    [JsonConstructor]
    public ResultViewModel(T result, IReadOnlyCollection<string> errors)
    {
        Result = result;

        if (errors is { Count: > 0 })
            _errors = errors as List<string> ?? errors.ToList();
    }

    public ResultViewModel(T result)
    {
        Result = result;
    }

    public ResultViewModel<T> AddError(string error)
        => AddErros(error);

    public ResultViewModel<T> AddErros(string erro, HttpStatusCode? status = null)
    {
        _errors ??= new List<string>(1);
        _errors.Add(erro);

        if (status is not null)
            StatusRetorno = status;

        return this;
    }

    public ResultViewModel<T> AddErros(IEnumerable<string> erros, HttpStatusCode? status = null)
    {
        _errors ??= new List<string>();
        _errors.AddRange(erros);

        if (status is not null)
            StatusRetorno = status;

        return this;
    }

    public ResultViewModel<T> AddResult(T valor, HttpStatusCode? status = null)
    {
        Result = valor;

        if (status is not null)
            StatusRetorno = status;

        return this;
    }
}