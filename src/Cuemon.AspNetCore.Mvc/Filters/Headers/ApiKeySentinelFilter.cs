using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Headers;
/// <summary>
/// A filter that confirms request authorization in the form of an API key sentinel.
/// </summary>
/// <seealso cref="ApiKeySentinelOptions"/>
/// <seealso cref="ConfigurableAsyncAuthorizationFilter{TOptions}"/>
public class ApiKeySentinelFilter : ConfigurableAsyncAuthorizationFilter<ApiKeySentinelOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeySentinelFilter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ApiKeySentinelOptions" /> which need to be configured.</param>
    public ApiKeySentinelFilter(IOptions<ApiKeySentinelOptions> setup) : base(setup)
    {
    }

    /// <summary>
    /// Called early in the filter pipeline to confirm request is authorized.
    /// </summary>
    /// <param name="context">The <see cref="AuthorizationFilterContext" />.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            await Decorator.Enclose(context.HttpContext).InvokeApiKeySentinelAsync(Options).ConfigureAwait(false);
        }
        catch (ApiKeyException ex)
        {
            context.Result = new ForbiddenObjectResult(ex.Message, ex.StatusCode);
        }

    }
}
