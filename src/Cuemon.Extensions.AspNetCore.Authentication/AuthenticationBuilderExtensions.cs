using System;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Microsoft.AspNetCore.Authentication;

namespace Cuemon.Extensions.AspNetCore.Authentication;
/// <summary>
/// Extension methods for the <see cref="AuthenticationBuilder"/> class.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds an <see cref="BasicAuthenticationHandler"/> to the authentication middleware.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="BasicAuthenticationOptions"/> which needs to be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="BasicAuthenticationOptions"/> in a valid state.
    /// </exception>
    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> setup)
    {
        Validator.ThrowIfNull(builder);
        Validator.ThrowIfInvalidConfigurator(setup, out _);
        return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthorizationHeader.Scheme, setup);
    }

    /// <summary>
    /// Adds an <see cref="DigestAuthenticationHandler"/> to the authentication middleware.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="DigestAuthenticationOptions"/> which needs to be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="DigestAuthenticationOptions"/> in a valid state.
    /// </exception>
    public static AuthenticationBuilder AddDigestAccess(this AuthenticationBuilder builder, Action<DigestAuthenticationOptions> setup)
    {
        Validator.ThrowIfNull(builder);
        Validator.ThrowIfInvalidConfigurator(setup, out _);
        return builder.AddScheme<DigestAuthenticationOptions, DigestAuthenticationHandler>(DigestAuthorizationHeader.Scheme, setup);
    }

    /// <summary>
    /// Adds an <see cref="HmacAuthenticationHandler"/> to the authentication middleware.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to extend.</param>
    /// <param name="setup">The <see cref="HmacAuthenticationOptions"/> which needs to be configured.</param>
    /// <returns>A reference to <paramref name="builder" /> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="HmacAuthenticationOptions"/> in a valid state.
    /// </exception>
    public static AuthenticationBuilder AddHmac(this AuthenticationBuilder builder, Action<HmacAuthenticationOptions> setup)
    {
        Validator.ThrowIfNull(builder);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return builder.AddScheme<HmacAuthenticationOptions, HmacAuthenticationHandler>(options.AuthenticationScheme, setup);
    }
}
