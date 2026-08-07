using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Hmac;
/// <summary>
/// Provides a HTTP HMAC Authentication middleware implementation for ASP.NET Core.
/// </summary>
public class HmacAuthenticationMiddleware : ConfigurableMiddleware<HmacAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HmacAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="HmacAuthenticationOptions" /> which need to be configured.</param>
    public HmacAuthenticationMiddleware(RequestDelegate next, IOptions<HmacAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HmacAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The middleware <see cref="HmacAuthenticationOptions"/> which need to be configured.</param>
    public HmacAuthenticationMiddleware(RequestDelegate next, Action<HmacAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="HmacAuthenticationMiddleware" />.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override async Task InvokeAsync(HttpContext context)
    {
        if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} cannot be null.")); }

        context.Items.TryAdd(nameof(HmacAuthenticationOptions), Options);

        if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate, out var principal))
        {
            await Decorator.Enclose(context).InvokeUnauthorizedExceptionAsync(Options, principal.Failure, dc => Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, Options.AuthenticationScheme)).ConfigureAwait(false);
        }

        context.User = principal.Result;

        await Next.Invoke(context).ConfigureAwait(false);
    }

    internal static bool TryAuthenticate(HttpContext context, HmacAuthorizationHeader header, out ConditionalValue<ClaimsPrincipal> result)
    {
        var options = context.Items[nameof(HmacAuthenticationOptions)] as HmacAuthenticationOptions;
        if (options?.Authenticator == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(options.Authenticator)} was unexpectedly set to null."));
            return false;
        }

        if (header == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(HmacAuthorizationHeader)} was unexpectedly passed as null."));
            return false;
        }

        var requestBodyMd5 = context.Request.Headers[HeaderNames.ContentMD5].FirstOrDefault()?.ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(requestBodyMd5) && !UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Md5).ComputeHash(context.Request.Body).ToHexadecimalString().Equals(requestBodyMd5, StringComparison.Ordinal))
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{HeaderNames.ContentMD5} header mismatch."));
            return false;
        }

        var clientId = header.ClientId;
        var presult = options.Authenticator(clientId, out var clientSecret);
        if (presult != null && clientSecret != null)
        {
            var signature = header.Signature;

            var hb = new HmacAuthorizationHeaderBuilder(options.AuthenticationScheme)
                .AddCredentialScope(header.CredentialScope)
                .AddClientId(clientId)
                .AddClientSecret(clientSecret)
                .AddSignedHeaders(header.SignedHeaders)
                .AddFromRequest(context.Request);

            var computedSignature = hb.ComputeSignature();
            if (computedSignature != null && signature.Equals(computedSignature, StringComparison.Ordinal))
            {
                result = new SuccessfulValue<ClaimsPrincipal>(presult);
                return true;
            }
        }

        result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"Unable to authenticate {header.ClientId}."));
        return false;
    }

    internal static HmacAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
    {
        var options = context.Items[nameof(HmacAuthenticationOptions)] as HmacAuthenticationOptions;
        return HmacAuthorizationHeader.Create(options!.AuthenticationScheme, authorizationHeader);
    }
}
