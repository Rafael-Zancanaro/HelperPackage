using Microsoft.AspNetCore.Mvc;
using PackageRZ.Domain.ViewModels;

namespace PackageRZ.Controllers;

/// <summary>
/// A base controller class that provides a standard method for returning view models as HTTP responses.
/// </summary>
public abstract class ControllerMain : ControllerBase
{
    /// <summary>
    /// Returns an appropriate HTTP response based on the success state of the provided view model.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="result">The view model result to evaluate.</param>
    /// <returns>An <see cref="IActionResult"/> representing a 200 OK or 400 Bad Request response.</returns>
    protected new IActionResult Response<T>(ResultViewModel<T> result)
        => result.Success
            ? Ok(result)
            : BadRequest(result);
}