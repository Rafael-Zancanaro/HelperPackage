using Microsoft.AspNetCore.Mvc;
using PackageRZ.Domain.ViewModels;

namespace PackageRZ.Controllers
{
    public abstract class ControllerMain : ControllerBase
    {
        protected new IActionResult Response<T>(ResultViewModel<T> result)
            => result.Success 
                ? Ok(result) 
                : BadRequest(result);
    }
}