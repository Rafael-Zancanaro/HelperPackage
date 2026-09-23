using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PackageRZ.Domain.Exceptions;
using PackageRZ.Logger;

namespace PackageRZ.Controllers.Handlers;

/// <summary>
/// Trata falhas de chamada HTTP as dependencias.
/// Loga status, metodo, URL e parametros SEM logar o conteudo/payload da resposta.
/// </summary>
public class TratadorIntegracaoHttpException(ILogger<TratadorIntegracaoHttpException> logger)
    : TratadorExcecaoBase<IntegracaoHttpException>
{
    protected override ValueTask<bool> TratarAsync(HttpContext httpContext, IntegracaoHttpException exception, CancellationToken cancellationToken)
    {
        // Conteudo omitido da escrita de log conforme regra de negocio
        logger.ErroHttp(
            exception,
            exception.EventId,
            exception.StatusCodeDependencia,
            exception.Metodo,
            exception.Url,
            parametros: exception.Parametros);

        return httpContext.EscreverErroPadraoAsync(exception.StatusRetorno, cancellationToken);
    }
}
