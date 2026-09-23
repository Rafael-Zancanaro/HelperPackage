using System.Net;

namespace PackageRZ.Domain.Exceptions;

public abstract class HelperException(
    int eventId,
    string mensagem,
    HttpStatusCode statusRetorno = HttpStatusCode.InternalServerError,
    Exception innerException = null) : Exception(mensagem, innerException)
{
    public int EventId { get; } = eventId;
    public HttpStatusCode StatusRetorno { get; } = statusRetorno;
}
