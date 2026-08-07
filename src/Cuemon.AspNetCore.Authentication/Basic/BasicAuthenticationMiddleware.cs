using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Basic;
/// <summary>
/// Provides a HTTP Basic Authentication middleware implementation for ASP.NET Core.
/// </summary>
public class BasicAuthenticationMiddleware : ConfigurableMiddleware<BasicAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The <see cref="BasicAuthenticationOptions" /> which need to be configured.</param>
    public BasicAuthenticationMiddleware(RequestDelegate next, IOptions<BasicAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate of the request pipeline to invoke.</param>
    /// <param name="setup">The middleware <see cref="BasicAuthenticationOptions"/> which need to be configured.</param>
    public BasicAuthenticationMiddleware(RequestDelegate next, Action<BasicAuthenticationOptions> setup) : base(next, setup)
    {
    }

    /// <summary>
    /// Executes the <see cref="BasicAuthenticationMiddleware"/>.
    /// </summary>
    /// <param name="context">The context of the current request.</param>
    /// <returns>A task that represents the execution of this middleware.</returns>
    public override async Task InvokeAsync(HttpContext context)
    {
        if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} cannot be null.")); }

        context.Items.TryAdd(nameof(BasicAuthenticationOptions), Options);

        if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate, out var principal))
        {
            await Decorator.Enclose(context).InvokeUnauthorizedExceptionAsync(Options, principal.Failure, dc => Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{BasicAuthorizationHeader.Scheme} realm=\"{Options.Realm}\""))).ConfigureAwait(false);
        }

        context.User = principal.Result;

        await Next(context).ConfigureAwait(false);
    }

    internal static bool TryAuthenticate(HttpContext context, BasicAuthorizationHeader header, out ConditionalValue<ClaimsPrincipal> result)
    {
        var options = context.Items[nameof(BasicAuthenticationOptions)] as BasicAuthenticationOptions;
        if (options?.Authenticator == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(options.Authenticator)} was unexpectedly set to null."));
            return false;
        }

        if (header == null)
        {
            result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{nameof(BasicAuthorizationHeader)} was unexpectedly passed as null."));
            return false;
        }

        var presult = options.Authenticator(header.UserName, header.Password);
        if (presult != null)
        {
            result = new SuccessfulValue<ClaimsPrincipal>(presult);
            return true;
        }

        result = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"Unable to authenticate {header.UserName}."));
        return false;
    }

    internal static BasicAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
    {
        return BasicAuthorizationHeader.Create(authorizationHeader);
    }
}
