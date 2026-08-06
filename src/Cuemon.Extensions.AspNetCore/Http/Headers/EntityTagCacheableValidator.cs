using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Data.Integrity;
using Cuemon.Diagnostics;
using Cuemon.IO;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Http.Headers;
/// <summary>
/// An HTTP validator that conforms to the ETag response header.
/// </summary>
/// <seealso cref="ICacheableValidator" />
public class EntityTagCacheableValidator : ICacheableValidator
{
    /// <summary>
    /// Called asynchronously before the <paramref name="bodyStream" /> is conditionally written to the response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> of the current request.</param>
    /// <param name="bodyStream">The intercepted <see cref="Stream" /> of the response body.</param>
    /// <returns>A <see cref="Task" /> that represents the execution of this validator.</returns>
    public Task ProcessAsync(HttpContext context, Stream bodyStream)
    {
        var serverTiming = context.RequestServices.GetService<IServerTiming>();
        Condition.FlipFlop(serverTiming == null, () => ComputeChecksum(context, bodyStream), () =>
        {
            var dynamicCacheTiming = TimeMeasure.WithAction(() => ComputeChecksum(context, bodyStream));
            serverTiming.AddServerTiming("entity-tag", dynamicCacheTiming.Elapsed);
        });
        return Task.CompletedTask;
    }

    private static void ComputeChecksum(HttpContext context, Stream bodyStream)
    {
        var builder = new ChecksumBuilder(Decorator.Enclose(bodyStream).InvokeToByteArray(leaveOpen: true), () => UnkeyedHashFactory.CreateCryptoMd5());
        context.Response.AddOrUpdateEntityTagHeader(context.Request, builder);
    }
}
