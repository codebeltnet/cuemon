using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// An <see cref="ObjectResult"/> that when executed will produce a Too Many Requests (429) response.
/// </summary>
public class TooManyRequestsObjectResult : ObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsObjectResult"/> class.
    /// </summary>
    /// <param name="error">Contains the errors to be returned to the client.</param>
    public TooManyRequestsObjectResult(object error) : base(error)
    {
        StatusCode = StatusCodes.Status429TooManyRequests;
    }
}
