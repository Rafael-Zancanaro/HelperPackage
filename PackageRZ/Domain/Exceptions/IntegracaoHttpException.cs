using PackageRZ.Domain.Config;
using PackageRZ.Utils;
using System.Net;

namespace PackageRZ.Domain.Exceptions;

/// <summary>
/// Falha em chamada HTTP para uma dependência.
/// Captura status, método, URL, query e corpo bruto da resposta no momento do lançamento.
/// </summary>
public class IntegracaoHttpException(
    int eventId,
    string url,
    string metodo,
    int statusCodeDependencia,
    string conteudo,
    string parametros,
    string mensagem,
    HttpStatusCode statusRetorno,
    Exception innerException)
    : HelperException(eventId, mensagem, statusRetorno, innerException)
{
    public string Url { get; } = url;
    public string Metodo { get; } = metodo;
    public int StatusCodeDependencia { get; } = statusCodeDependencia;
    public string Conteudo { get; } = conteudo;
    public string Parametros { get; } = parametros;

    /// <summary>
    /// Cria a exception a partir da resposta, lendo o corpo bruto antes que ele seja descartado.
    /// </summary>
    public static async Task<IntegracaoHttpException> CriarAsync(
        HttpResponseMessage responseMessage,
        Exception innerException = null)
    {
        ArgumentNullException.ThrowIfNull(responseMessage);

        var conteudo = responseMessage.Content is null
            ? null
            : await responseMessage.Content.ReadAsStringAsync();

        var requestMessage = responseMessage.RequestMessage;
        var url = requestMessage?.RequestUri?.ToString();
        var metodo = requestMessage?.Method?.ToString();
        var statusCode = (int)responseMessage.StatusCode;
        var parametros = await requestMessage.ObterParametrosAsync();

        return new IntegracaoHttpException(
            LogConstantes.EventIdErroNaoTratado,
            url,
            metodo,
            statusCode,
            conteudo,
            parametros,
            $"Falha na chamada HTTP: {metodo} {url} respondeu {statusCode}.",
            HttpStatusCode.InternalServerError,
            innerException);
    }

    /// <summary>
    /// Cria a exception quando uma chamada é recusada por um circuit breaker aberto.
    /// </summary>
    public static async Task<IntegracaoHttpException> CriarCircuitoAbertoAsync(
        HttpRequestMessage requestMessage,
        Exception innerException)
    {
        ArgumentNullException.ThrowIfNull(requestMessage);
        ArgumentNullException.ThrowIfNull(innerException);

        var url = requestMessage.RequestUri?.ToString();
        var metodo = requestMessage.Method?.ToString();
        var parametros = await requestMessage.ObterParametrosAsync();

        return new IntegracaoHttpException(
            LogConstantes.EventIdErroNaoTratado,
            url,
            metodo,
            (int)HttpStatusCode.InternalServerError,
            null,
            parametros,
            $"Circuit breaker aberto para a chamada HTTP: {metodo} {url}.",
            HttpStatusCode.InternalServerError,
            innerException);
    }
}
