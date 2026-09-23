using System.Net;

namespace PackageRZ.Domain.Models;

public readonly struct ResultFalha
{
    public IEnumerable<string> Errors { get; }
    public HttpStatusCode? Status { get; }

    public ResultFalha(IEnumerable<string> errors, HttpStatusCode? status)
    {
        Errors = errors;
        Status = status;
    }

    public ResultFalha(string error, HttpStatusCode? status)
    {
        Errors = [error];
        Status = status;
    }
}
