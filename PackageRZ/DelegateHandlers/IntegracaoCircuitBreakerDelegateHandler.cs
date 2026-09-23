using PackageRZ.Domain.Exceptions;
using Polly.CircuitBreaker;

namespace PackageRZ.DelegateHandlers;

/// <summary>
/// Converte a excecao do circuit breaker em uma excecao de integracao,
/// preservando os dados da requisicao que foi bloqueada.
/// </summary>
public sealed class IntegracaoCircuitBreakerDelegateHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (BrokenCircuitException exception)
        {
            throw await IntegracaoHttpException.CriarCircuitoAbertoAsync(request, exception);
        }
    }
}
