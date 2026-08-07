using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Digest;
/// <summary>
/// Provides a HTTP Digest Access Authentication middleware implementation for ASP.NET Core.
/// </summary>
public class DigestAuthenticationMiddleware : ConfigurableMiddleware<INonceTracker, DigestAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="DigestAuthenticationOptions" /> which need to be configured.</param>
    public DigestAuthenticationMiddleware(RequestDelegate next, IOptions<DigestAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The middleware <see cref="DigestAuthenticationOptions"/> which need to be configured.</param>
    public DigestAuthenticationMiddleware(RequestDelegate next, Action<DigestAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="DigestAuthenticationMiddleware"/>.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <param name="di">The dependency injected implementation of an <see cref="INonceTracker"/>.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    /// <remarks><c>qop</c> is included and supported to be compliant with RFC 2617 (hence, this implementation cannot revert to reduced legacy RFC 2069 mode).</remarks>
    public override async Task InvokeAsync(HttpContext context, INonceTracker di)
    {
        if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} delegate cannot be null.")); }

        context.Items.TryAdd(nameof(DigestAuthenticationOptions), Options);
        context.Items.TryAdd(nameof(INonceTracker), di);

        if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate, out var principal))
        {
            await Decorator.Enclose(context).InvokeUnauthorizedExceptionAsync(Options, principal.Failure, dc =>
            {
                string etag = dc.Response.Headers[HeaderNames.ETag];
                if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
                var opaqueGenerator = Options.OpaqueGenerator;
                var nonceSecret = Options.NonceSecret;
                var nonceGenerator = Options.NonceGenerator;
                var staleNonce = dc.Items[DigestFields.Stale] as string ?? "false";
                Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{DigestAuthorizationHeader.Scheme} realm=\"{Options.Realm}\", qop=\"auth, auth-int\", nonce=\"{nonceGenerator(DateTime.UtcNow, etag, nonceSecret())}\", opaque=\"{opaqueGenerator()}\", stale={staleNonce}, algorithm={ParseAlgorithm(Options.DigestAlgorithm)}"));
            }).ConfigureAwait(false);
        }

        context.User = principal.Result;
        await Next.Invoke(context).ConfigureAwait(false);
    }

    internal static bool TryAuthenticate(HttpContext context, DigestAuthorizationHeader header, out ConditionalValue<ClaimsPrincipal> result)
    {
        var options = context.Items[nameof(DigestAuthenticationOptions)] as DigestAuthenticationOptions;
        if (options?.Authenticator == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(options.Authenticator)} was unexpectedly set to null."));
            return false;
        }

        if (header == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(DigestAuthorizationHeader)} was unexpectedly passed as null."));
            return false;
        }

        var nonceExpiredParser = options.NonceExpiredParser;
        var staleNonce = nonceExpiredParser(header.Nonce, TimeSpan.FromSeconds(30));
        if (staleNonce)
        {
            context.Items.Add(DigestFields.Stale, "true");
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException("Stale nonce detected."));
            return false;
        }

        if (context.Items[nameof(INonceTracker)] is INonceTracker nonceTracker)
        {
            var nc = Convert.ToInt32(header.NC, 16);
            if (nonceTracker.TryGetEntry(header.Nonce, out var previousNonce))
            {
                if (previousNonce.Count == nc)
                {
                    context.Items.Add(DigestFields.Stale, "true");
                    result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException("Nonce replay detected."));
                    return false;
                }
            }
            else
            {
                nonceTracker.TryAddEntry(header.Nonce, nc);
            }
        }

        var presult = options.Authenticator(header.UserName, out var password);
        if (presult != null)
        {
            context.Request.EnableBuffering();

            using var body = new MemoryStream();
            context.Request.Body.CopyToAsync(body, context.RequestAborted).GetAwaiter().GetResult();
            var db = new DigestAuthorizationHeaderBuilder().AddFromDigestAuthorizationHeader(header);
            var ha1 = options.UseServerSideHa1Storage ? password : db.ComputeHash1(password);
            var ha2 = db.ComputeHash2(context.Request.Method, Decorator.Enclose(body).ToEncodedString());
            var serverResponse = db.ComputeResponse(ha1, ha2);
            if (serverResponse != null && serverResponse.Equals(header.Response, StringComparison.OrdinalIgnoreCase))
            {
                result = new SuccessfulValue<ClaimsPrincipal>(presult);
                return true;
            }
        }

        result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"Unable to authenticate {header.UserName}."));
        return false;
    }

    internal static DigestAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
    {
        return DigestAuthorizationHeader.Create(authorizationHeader);
    }

    internal static string ParseAlgorithm(DigestCryptoAlgorithm algorithm)
    {
        switch (algorithm)
        {
            case DigestCryptoAlgorithm.Md5:
                return "MD5";
            case DigestCryptoAlgorithm.Md5Session:
                return "MD5-sess";
            case DigestCryptoAlgorithm.Sha256:
                return "SHA-256";
            case DigestCryptoAlgorithm.Sha256Session:
                return "SHA-256-sess";
            case DigestCryptoAlgorithm.Sha512Slash256:
                return "SHA-512-256";
            case DigestCryptoAlgorithm.Sha512Slash256Session:
                return "SHA-512-256-sess";
            default:
                return "MD5";
        }
    }
}
