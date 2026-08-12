using System;
using System.Threading.Tasks;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters;
/// <summary>
/// A base class implementation of a filter that asynchronously surrounds execution of the action, after model binding is complete.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IAsyncActionFilter" />
public abstract class ConfigurableAsyncActionFilter<TOptions> : Configurable<TOptions>, IAsyncActionFilter where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableAsyncActionFilter(Action<TOptions> setup) : base(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableAsyncActionFilter(IOptions<TOptions> setup) : base(setup.Value)
    {
    }

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public abstract Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);
}
