using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// A filter that will invoke filters implementing the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
/// <seealso cref="ConfigurableAsyncResultFilter{HttpCacheableOptions}" />
public class HttpCacheableFilter : ConfigurableAsyncResultFilter<HttpCacheableOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpCacheableFilter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="HttpCacheableOptions" /> which need to be configured.</param>
    public HttpCacheableFilter(IOptions<HttpCacheableOptions> setup) : base(setup)
    {
    }

    /// <summary>
    /// Called asynchronously before the action result.
    /// </summary>
    /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
    /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        foreach (var filter in Options.Filters)
        {
            await filter.OnResultExecutionAsync(context, next).ConfigureAwait(false);
        }

        if (context.Result is ObjectResult result && result.Value is ICacheableObjectResult cacheableObjectResult)
        {
            result.Value = cacheableObjectResult.Value;
            cacheableObjectResult.Value = null;
        }

        if (!context.HttpContext.Response.HasStarted)
        {
            if (Options.UseCacheControl) { context.HttpContext.Response.GetTypedHeaders().CacheControl = Options.CacheControl; }
            if (context.HttpContext.Response.StatusCode != StatusCodes.Status304NotModified) { await next().ConfigureAwait(false); }
        }
    }
}
