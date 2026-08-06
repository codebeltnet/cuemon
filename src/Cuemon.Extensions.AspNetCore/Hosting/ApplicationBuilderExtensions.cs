using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Hosting;
/// <summary>
/// Extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a hosting environment HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <param name="setup">The <see cref="HostingEnvironmentOptions"/> middleware which may be configured.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>Default HTTP header name is <c>X-Hosting-Environment</c>.</remarks>
    public static IApplicationBuilder UseHostingEnvironment(this IApplicationBuilder builder, Action<HostingEnvironmentOptions> setup = null)
    {
        return MiddlewareBuilderFactory.UseConfigurableMiddleware<HostingEnvironmentMiddleware, HostingEnvironmentOptions>(builder, setup);
    }
}
