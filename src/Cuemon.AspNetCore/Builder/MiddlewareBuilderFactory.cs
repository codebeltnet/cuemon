using System;
using Cuemon.AspNetCore.Infrastructure;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.AspNetCore.Builder;
/// <summary>
/// Provides support for creating, using and configuring <see cref="Middleware" /> or <see cref="ConfigurableMiddleware{TOptions}"/> implementations.
/// </summary>
public static class MiddlewareBuilderFactory
{
    /// <summary>
    /// Adds a middleware type to the application request pipeline.
    /// </summary>
    /// <typeparam name="TMiddleware">The type of the middleware.</typeparam>
    /// <param name="builder">The <see cref="IApplicationBuilder" /> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder" /> instance.</returns>
    public static IApplicationBuilder UseMiddleware<TMiddleware>(IApplicationBuilder builder) where TMiddleware : MiddlewareCore
    {
        return builder.UseMiddleware<TMiddleware>();
    }

    /// <summary>
    /// Adds a configurable middleware type to the application request pipeline.
    /// </summary>
    /// <typeparam name="TMiddleware">The type of the configurable middleware.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <param name="builder">The <see cref="IApplicationBuilder" /> instance.</param>
    /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
    /// <returns>The <see cref="IApplicationBuilder" /> instance.</returns>
    public static IApplicationBuilder UseConfigurableMiddleware<TMiddleware, TOptions>(IApplicationBuilder builder, Action<TOptions> setup = null)
        where TMiddleware : ConfigurableMiddlewareCore<TOptions>
        where TOptions : class, IParameterObject, new()
    {
        return setup == null
            ? builder.UseMiddleware<TMiddleware>()
            : builder.UseMiddleware<TMiddleware>(Validator.CheckParameter(setup, () =>
            {
                Validator.ThrowIfInvalidConfigurator(setup, out _);
            }));
    }
}
