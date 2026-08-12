using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// An <see cref="ObjectResult"/> that when executed will produce a Forbidden (403) response.
/// </summary>
public class ForbiddenObjectResult : ObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenObjectResult"/> class.
    /// </summary>
    /// <param name="value">The value to be returned to the client.</param>
    /// <param name="statusCode">The HTTP status code of the response which has to be in the 400-499 range. Default is 403, but for security reasons you may wish to "hide" this with another, e.g., 400, 404 or whatever fits your strategy.</param>
    /// <remarks>https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.4</remarks>
    public ForbiddenObjectResult(object value, int statusCode = StatusCodes.Status403Forbidden) : base(value)
    {
        Validator.ThrowIfFalse(Decorator.Enclose(statusCode).IsClientErrorStatusCode(), nameof(statusCode));
        StatusCode = statusCode;
    }
}
