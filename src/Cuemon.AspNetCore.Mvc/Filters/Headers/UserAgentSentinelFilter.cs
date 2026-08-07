using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Headers;
/// <summary>
/// A filter that provides an User-Agent sentinel on action methods.
/// </summary>
/// <seealso cref="ConfigurableAsyncActionFilter{TOptions}"/>
/// <seealso cref="IAsyncActionFilter" />
public class UserAgentSentinelFilter : ConfigurableAsyncActionFilter<UserAgentSentinelOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAgentSentinelFilter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="UserAgentSentinelOptions" /> which need to be configured.</param>
    public UserAgentSentinelFilter(IOptions<UserAgentSentinelOptions> setup) : base(setup)
    {
    }

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await Decorator.Enclose(context.HttpContext).InvokeUserAgentSentinelAsync(Options).ConfigureAwait(false);
        await next().ConfigureAwait(false);
    }
}
