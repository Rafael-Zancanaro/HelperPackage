using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackageRZ.Domain.ViewModels;
using PackageRZ.Utils;
using System.Net;

namespace PackageRZ.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("error")]
        public async Task<ResultViewModel<string>> Error()
        {
            var result = new ResultViewModel<string>();
            result.AddError(HelperResources.InternalError);
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError(exception.Error, HelperResources.Informations, 
                string.Format(HelperResources.LogInformation, DateTime.Now, HttpContext.User.Identity.Name));
            Response.StatusCode = (short)HttpStatusCode.InternalServerError;

            return result;
        }
    }
}