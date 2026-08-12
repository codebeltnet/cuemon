using System;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Authentication;
/// <summary>
/// Extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a HTTP Basic Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <param name="setup">The HTTP <see cref="BasicAuthenticationOptions"/> middleware which may be configured.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder, Action<BasicAuthenticationOptions> setup = null)
    {
        return MiddlewareBuilderFactory.UseConfigurableMiddleware<BasicAuthenticationMiddleware, BasicAuthenticationOptions>(builder, setup);
    }

    /// <summary>
    /// Adds a HTTP Digest Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <param name="setup">The HTTP <see cref="DigestAuthenticationMiddleware"/> middleware which may be configured.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static IApplicationBuilder UseDigestAccessAuthentication(this IApplicationBuilder builder, Action<DigestAuthenticationOptions> setup = null)
    {
        return MiddlewareBuilderFactory.UseConfigurableMiddleware<DigestAuthenticationMiddleware, DigestAuthenticationOptions>(builder, setup);
    }

    /// <summary>
    /// Adds a HTTP HMAC Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
    /// </summary>
    /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
    /// <param name="setup">The HTTP <see cref="HmacAuthenticationOptions"/> middleware which may be configured.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static IApplicationBuilder UseHmacAuthentication(this IApplicationBuilder builder, Action<HmacAuthenticationOptions> setup = null)
    {
        return MiddlewareBuilderFactory.UseConfigurableMiddleware<HmacAuthenticationMiddleware, HmacAuthenticationOptions>(builder, setup);
    }
}
