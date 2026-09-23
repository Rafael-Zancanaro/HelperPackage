using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace PackageRZ.Controllers.Handlers;

/// <summary>
/// Base dos tratadores de exception. Resolve o teste de tipo e delega para a implementacao especifica.
/// </summary>
public abstract class TratadorExcecaoBase<TException> : IExceptionHandler where TException : Exception
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        => exception is TException excecao
            ? TratarAsync(httpContext, excecao, cancellationToken)
            : ValueTask.FromResult(false);

    protected abstract ValueTask<bool> TratarAsync(HttpContext httpContext, TException exception, CancellationToken cancellationToken);
}
