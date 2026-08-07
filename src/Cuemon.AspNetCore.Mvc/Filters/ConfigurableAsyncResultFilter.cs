using System;
using System.Threading.Tasks;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters;
/// <summary>
/// A base class implementation of a filter that asynchronously surrounds execution of action results successfully returned from an action.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IAsyncResultFilter" />
public abstract class ConfigurableAsyncResultFilter<TOptions> : Configurable<TOptions>, IAsyncResultFilter where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableAsyncResultFilter(Action<TOptions> setup) : base(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableAsyncResultFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableAsyncResultFilter(IOptions<TOptions> setup) : base(setup.Value)
    {
    }

    /// <summary>
    /// Called asynchronously before the action result.
    /// </summary>
    /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
    /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
    public abstract Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next);
}
