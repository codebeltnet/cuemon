using System;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Infrastructure;
/// <summary>
/// Provides an base-class for configurable middleware implementation in ASP.NET Core that supports the options pattern.
/// This API supports the product infrastructure and is not intended to be used directly from your code.
/// </summary>
/// <typeparam name="TOptions">The type of the options to setup.</typeparam>
/// <seealso cref="Middleware" />
public abstract class ConfigurableMiddlewareCore<TOptions> : MiddlewareCore, IConfigurable<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    internal ConfigurableMiddlewareCore(RequestDelegate next, Action<TOptions> setup) : base(next)
    {
        Options = Patterns.Configure(setup);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Middleware{TOptions}"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="IOptions{TOptions}"/> which need to be configured.</param>
    internal ConfigurableMiddlewareCore(RequestDelegate next, IOptions<TOptions> setup) : base(next)
    {
        Options = setup.Value;
    }

    /// <summary>
    /// Gets the configured options of this <see cref="Middleware"/>.
    /// </summary>
    /// <value>The configured options of this <see cref="Middleware"/>.</value>
    public TOptions Options { get; }
}
