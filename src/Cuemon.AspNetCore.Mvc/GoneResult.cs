using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// An <see cref="ActionResult" /> that returns a Gone (410) response.
/// </summary>
public class GoneResult : StatusCodeResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GoneResult"/> class.
    /// </summary>
    public GoneResult() : base(StatusCodes.Status410Gone)
    {
    }
}
