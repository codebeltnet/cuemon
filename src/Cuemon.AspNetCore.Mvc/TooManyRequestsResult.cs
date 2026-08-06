using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// An <see cref="ActionResult" /> that returns a TooManyRequests (429) response.
/// </summary>
public class TooManyRequestsResult : StatusCodeResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsResult"/> class.
    /// </summary>
    public TooManyRequestsResult() : base(StatusCodes.Status429TooManyRequests)
    {
    }
}
