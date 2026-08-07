using System;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters;
/// <summary>
/// A base class implementation of a filter metadata which can create an instance of an executable filter.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="Configurable{TOptions}" />
/// <seealso cref="IFilterFactory" />
public abstract class ConfigurableFactoryFilter<TOptions> : Configurable<TOptions>, IFilterFactory where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableFactoryFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    protected ConfigurableFactoryFilter(Action<TOptions> setup) : base(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableFactoryFilter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    protected ConfigurableFactoryFilter(IOptions<TOptions> setup) : base(setup.Value)
    {
    }

    /// <summary>
    /// Creates an instance of the executable filter.
    /// </summary>
    /// <param name="serviceProvider">The request <see cref="IServiceProvider" />.</param>
    /// <returns>An instance of the executable filter.</returns>
    public abstract IFilterMetadata CreateInstance(IServiceProvider serviceProvider);

    /// <summary>
    /// Gets a value that indicates if the result of <see cref="IFilterFactory.CreateInstance" /> can be reused across requests.
    /// </summary>
    /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
    public virtual bool IsReusable => false;
}
