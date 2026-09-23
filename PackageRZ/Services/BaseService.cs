using PackageRZ.Domain.Models;
using PackageRZ.Domain.ViewModels;
using System.Net;

namespace PackageRZ.Services;

public abstract class BaseService
{
    protected static ResultFalha AddErros(string erro, HttpStatusCode? status = null)
        => new(erro, status);

    protected static ResultFalha AddErros(IEnumerable<string> erros, HttpStatusCode? status = null)
        => new(erros, status);

    protected static ResultViewModel<T> AddResult<T>(T result, HttpStatusCode? status = null)
        => new ResultViewModel<T>().AddResult(result, status);
}
