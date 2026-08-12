using System.Threading.Tasks;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters;
/// <summary>
/// A base class implementation of a filter that asynchronously confirms request authorization.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IAsyncAuthorizationFilter" />
public abstract class ConfigurableAsyncAuthorizationFilter<TOptions> : Configurable<TOptions>, IAsyncAuthorizationFilter where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncAuthorizationFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableAsyncAuthorizationFilter(IOptions<TOptions> setup) : base(setup.Value)
    {
    }

    /// <summary>
    /// Called early in the filter pipeline to confirm request is authorized.
    /// </summary>
    /// <param name="context">The <see cref="AuthorizationFilterContext" />.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);
}
