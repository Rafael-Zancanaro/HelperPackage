using Microsoft.AspNetCore.Http;
using PackageRZ.Domain.ViewModels;
using System.Net;

namespace PackageRZ.Controllers.Handlers;

public static class RespostaErroExtensions
{
    public static async ValueTask<bool> EscreverErroPadraoAsync(
        this HttpContext httpContext,
        HttpStatusCode status,
        CancellationToken cancellationToken)
    {
        if (httpContext.Response.HasStarted)
            return false;

        httpContext.Response.StatusCode = (int)status;

        await httpContext.Response.WriteAsJsonAsync(CriarErroPadrao(), cancellationToken);

        return true;
    }

    internal static ResultViewModel<bool> CriarErroPadrao()
        => new ResultViewModel<bool>().AddErros(PackageRZ.Utils.HelperResources.InternalError);
}
