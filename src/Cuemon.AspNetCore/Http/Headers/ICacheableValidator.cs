using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// An HTTP validator tailored for cacheable flows, that asynchronously surrounds execution of the intercepted response body.
/// </summary>
public interface ICacheableValidator
{
    /// <summary>
    /// Called asynchronously before the <paramref name="bodyStream"/> is conditionally written to the response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> of the current request.</param>
    /// <param name="bodyStream">The intercepted <see cref="Stream" /> of the response body.</param>
    /// <returns>A <see cref="Task"/> that represents the execution of this validator.</returns>
    /// <remarks><paramref name="bodyStream"/> is written to the response body if condition is not equal to a 304 status code.</remarks>
    Task ProcessAsync(HttpContext context, Stream bodyStream);
}
