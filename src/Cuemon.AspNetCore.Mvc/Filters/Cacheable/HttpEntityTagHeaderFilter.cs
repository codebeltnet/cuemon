using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// A filter that computes the response body and applies an appropriate HTTP Etag header.
/// </summary>
/// <seealso cref="HttpCacheableFilter" />
public class HttpEntityTagHeaderFilter : IConfigurable<HttpEntityTagHeaderOptions>, ICacheableAsyncResultFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpEntityTagHeaderFilter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="HttpEntityTagHeaderOptions" /> which may be configured.</param>
    public HttpEntityTagHeaderFilter(Action<HttpEntityTagHeaderOptions> setup = null)
    {
        Options = Patterns.Configure(setup);
    }

    /// <summary>
    /// Called asynchronously before the action result.
    /// </summary>
    /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
    /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var useFallbackToEntityTagResponseParser = true;
        if (Options.HasEntityTagProvider &&
            Decorator.Enclose(context.HttpContext.Request).IsGetOrHeadMethod() &&
            (Decorator.Enclose(context.HttpContext.Response.StatusCode).IsSuccessStatusCode() || Decorator.Enclose(context.HttpContext.Response.StatusCode).IsNotModifiedStatusCode()))
        {
            if (context.Result is ObjectResult result && result.Value is IEntityDataIntegrity integrity && integrity.Checksum.HasValue)
            {
                useFallbackToEntityTagResponseParser = false;
                Options.EntityTagProvider.Invoke(integrity, context.HttpContext);
            }
        }

        if (useFallbackToEntityTagResponseParser &&
            Options.HasEntityTagResponseParser &&
            Options.UseEntityTagResponseParser &&
            Decorator.Enclose(context.HttpContext.Request).IsGetOrHeadMethod() &&
            (Decorator.Enclose(context.HttpContext.Response.StatusCode).IsSuccessStatusCode() || Decorator.Enclose(context.HttpContext.Response.StatusCode).IsNotModifiedStatusCode()))
        {
            var statusCodeBeforeBodyRead = context.HttpContext.Response.StatusCode;
            if (context.Result is ObjectResult result && result.Value is ICacheableObjectResult cacheableObjectResult)
            {
                var originalValue = result.Value;
                result.Value = cacheableObjectResult.Value;
                await InvokeEntityTagResponseParser(context, next, statusCodeBeforeBodyRead).ConfigureAwait(false);
                result.Value = originalValue;
            }
            else
            {
                await InvokeEntityTagResponseParser(context, next, statusCodeBeforeBodyRead).ConfigureAwait(false);
            }
        }
    }

    private async Task InvokeEntityTagResponseParser(ResultExecutingContext context, ResultExecutionDelegate next, int statusCodeBeforeBodyRead)
    {
        var ms = new MemoryStream();
        var body = context.HttpContext.Response.Body;
        context.HttpContext.Response.Body = ms;
        await next().ConfigureAwait(false);
        ms.Seek(0, SeekOrigin.Begin);

        if (statusCodeBeforeBodyRead == StatusCodes.Status304NotModified) { context.HttpContext.Response.StatusCode = statusCodeBeforeBodyRead; }

        Options.EntityTagResponseParser.Invoke(ms, context.HttpContext.Request, context.HttpContext.Response);
        if (Decorator.Enclose(context.HttpContext.Response.StatusCode).IsSuccessStatusCode())
        {
            await ms.CopyToAsync(body).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public HttpEntityTagHeaderOptions Options { get; }
}
