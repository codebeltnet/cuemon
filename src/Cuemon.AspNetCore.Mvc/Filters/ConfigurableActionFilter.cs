using System;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters;
/// <summary>
/// A base class implementation of a filter that surrounds execution of the action.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IActionFilter" />
public abstract class ConfigurableActionFilter<TOptions> : Configurable<TOptions>, IActionFilter where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableActionFilter(Action<TOptions> setup) : base(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableActionFilter(IOptions<TOptions> setup) : base(setup.Value)
    {
    }

    /// <summary>
    /// Called before the action executes, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
    public abstract void OnActionExecuting(ActionExecutingContext context);

    /// <summary>
    /// Called after the action executes, before the action result.
    /// </summary>
    /// <param name="context">The <see cref="ActionExecutedContext" />.</param>
    public abstract void OnActionExecuted(ActionExecutedContext context);
}
