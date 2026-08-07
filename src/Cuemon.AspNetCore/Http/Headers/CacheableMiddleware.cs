using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Provides a Cache-Control middleware implementation for ASP.NET Core.
/// </summary>
public class CacheableMiddleware : ConfigurableMiddleware<CacheableOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="CacheableOptions" /> which need to be configured.</param>
    public CacheableMiddleware(RequestDelegate next, IOptions<CacheableOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="CacheableOptions" /> which need to be configured.</param>
    public CacheableMiddleware(RequestDelegate next, Action<CacheableOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="CacheableMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
    public override async Task InvokeAsync(HttpContext context)
    {
        if (Options.UseCacheControl) { context.Response.GetTypedHeaders().CacheControl = Options.CacheControl; }
        if (Options.UseExpires) { context.Response.Headers[HeaderNames.Expires] = Options.Expires.ToString(); }

        using (var bodyStream = new MemoryStream())
        {
            var body = context.Response.Body;
            context.Features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(bodyStream));

            var serverTiming = context.RequestServices.GetService(typeof(IServerTiming)) as IServerTiming;
            await Condition.FlipFlopAsync(serverTiming == null, () => Next(context), async () =>
            {
                var requestTiming = await TimeMeasure.WithActionAsync(async _ => await Next(context).ConfigureAwait(false)).ConfigureAwait(false);
                serverTiming.AddServerTiming("entity-body", requestTiming.Elapsed);
            }).ConfigureAwait(false);

            foreach (var validator in Options.Validators)
            {
                bodyStream.Seek(0, SeekOrigin.Begin);
                await validator.ProcessAsync(context, bodyStream);
            }

            if (!Decorator.Enclose(context.Response.StatusCode).IsNotModifiedStatusCode()) { await bodyStream.CopyToAsync(body, context.RequestAborted).ConfigureAwait(false); }
        }
    }
}
