using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling;
/// <summary>
/// Extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a HTTP requests rate limiting / throttling guard to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> middleware which may be configured.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static IApplicationBuilder UseThrottlingSentinel(this IApplicationBuilder builder, Action<ThrottlingSentinelOptions> setup = null)
    {
        return MiddlewareBuilderFactory.UseConfigurableMiddleware<ThrottlingSentinelMiddleware, ThrottlingSentinelOptions>(builder, setup);
    }
}
