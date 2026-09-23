using Microsoft.Extensions.Logging;
using PackageRZ.Utils;
using static PackageRZ.Domain.Config.Constantes;
using static PackageRZ.Domain.Config.LogConstantes;

namespace PackageRZ.Logger;

/// <summary>
/// Classe centralizada de log estruturado para PackageRZ.
/// Utiliza LoggerMessage source generators para zero-alocacao quando o nivel de log esta desabilitado.
/// </summary>
public static partial class HelperLogger
{
    public static void Erro(this ILogger logger, int eventId, string mensagem = NaoEspecificado, string detalhes = NaoEspecificado, string origem = NaoEspecificado)
        => LogErro(logger, eventId, origem ?? NaoEspecificado, mensagem, detalhes);

    public static void Erro(this ILogger logger, Exception exception, int eventId, string mensagem = NaoEspecificado, string detalhes = NaoEspecificado, string origem = NaoEspecificado)
        => LogErroException(logger, exception, eventId, origem ?? NaoEspecificado, mensagem, detalhes);

    public static void Aviso(this ILogger logger, int eventId, string mensagem = NaoEspecificado, string detalhes = NaoEspecificado, string origem = NaoEspecificado)
        => LogAviso(logger, eventId, origem ?? NaoEspecificado, mensagem, detalhes);

    public static async Task ErroHttpAsync(this ILogger logger, int eventId, HttpResponseMessage responseMessage, string origem = NaoEspecificado)
    {
        LogErroHttp(
            logger,
            eventId,
            origem ?? NaoEspecificado,
            (int)responseMessage.StatusCode,
            responseMessage.RequestMessage?.Method.ToString() ?? NaoEspecificado,
            responseMessage.RequestMessage?.RequestUri?.ToString() ?? NaoEspecificado,
            await responseMessage.RequestMessage.ObterParametrosAsync() ?? NaoEspecificado,
            await responseMessage.Content.ReadAsStringAsync()
        );
    }

    public static void ErroHttp(this ILogger logger, Exception exception, int eventId, int statusCode, string metodo, string url, string conteudo, string parametros = NaoEspecificado, string origem = NaoEspecificado)
        => LogErroHttpException(
            logger,
            exception,
            eventId,
            origem ?? NaoEspecificado,
            statusCode,
            metodo ?? NaoEspecificado,
            url ?? NaoEspecificado,
            parametros ?? NaoEspecificado,
            conteudo ?? NaoEspecificado);

    public static void ErroHttp(this ILogger logger, Exception exception, int eventId, int statusCode, string metodo, string url, string parametros = NaoEspecificado, string origem = NaoEspecificado)
        => LogErroHttpExceptionSemConteudo(
            logger,
            exception,
            eventId,
            origem ?? NaoEspecificado,
            statusCode,
            metodo ?? NaoEspecificado,
            url ?? NaoEspecificado,
            parametros ?? NaoEspecificado);

    public static async Task AvisoHttpAsync(this ILogger logger, int eventId, HttpResponseMessage responseMessage, string origem = NaoEspecificado)
    {
        LogAvisoHttp(
            logger,
            eventId,
            origem ?? NaoEspecificado,
            (int)responseMessage.StatusCode,
            responseMessage.RequestMessage?.Method.ToString() ?? NaoEspecificado,
            responseMessage.RequestMessage?.RequestUri?.ToString() ?? NaoEspecificado,
            await responseMessage.RequestMessage.ObterParametrosAsync() ?? NaoEspecificado,
            await responseMessage.Content.ReadAsStringAsync()
        );
    }

    [LoggerMessage(Level = LogLevel.Error, Message = TemplateMensagem)]
    private static partial void LogErro(ILogger logger, int eventId, string origem, string mensagem, string detalhes);

    [LoggerMessage(Level = LogLevel.Error, Message = TemplateMensagem)]
    private static partial void LogErroException(ILogger logger, Exception exception, int eventId, string origem, string mensagem, string detalhes);

    [LoggerMessage(Level = LogLevel.Warning, Message = TemplateMensagem)]
    private static partial void LogAviso(ILogger logger, int eventId, string origem, string mensagem, string detalhes);

    [LoggerMessage(Level = LogLevel.Error, Message = TemplateHttp)]
    private static partial void LogErroHttp(ILogger logger, int eventId, string origem, int statusCode, string method, string url, string parametros, string conteudo);

    [LoggerMessage(Level = LogLevel.Error, Message = TemplateHttp)]
    private static partial void LogErroHttpException(ILogger logger, Exception exception, int eventId, string origem, int statusCode, string method, string url, string parametros, string conteudo);

    [LoggerMessage(Level = LogLevel.Error, Message = TemplateHttpSemConteudo)]
    private static partial void LogErroHttpExceptionSemConteudo(ILogger logger, Exception exception, int eventId, string origem, int statusCode, string method, string url, string parametros);

    [LoggerMessage(Level = LogLevel.Warning, Message = TemplateHttp)]
    private static partial void LogAvisoHttp(ILogger logger, int eventId, string origem, int statusCode, string method, string url, string parametros, string conteudo);
}
