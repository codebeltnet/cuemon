using System;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Provides a set of static methods for working with HTTP based authentication.
/// </summary>
public static class Authenticator
{
    /// <summary>
    /// Provides a generic way to make authentication requests using the specified <paramref name="context"/>.
    /// </summary>
    /// <typeparam name="T">The type of the credentials returned from <paramref name="authorizationParser"/> and passed to <paramref name="principalParser"/>.</typeparam>
    /// <param name="context">The context of the ASP.NET application.</param>
    /// <param name="requireSecureConnection">When <c>true</c>, the HTTP connection is required to use secure sockets (that is, HTTPS); when <c>false</c> no requirement is enforced.</param>
    /// <param name="authorizationParser">The function delegate that will parse the authorization header of a web request and return the credentials of <typeparamref name="T"/>.</param>
    /// <param name="principalParser">The function delegate that will parse the credentials of <typeparamref name="T"/> returned from <paramref name="authorizationParser"/> and if successful returns a <see cref="ClaimsPrincipal"/> object.</param>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> of the authenticated user if the authentication was successful.</param>
    /// <returns><c>true</c> if the specified parameters triggers a successful authentication; otherwise, <c>false</c>.</returns>
    public static bool TryAuthenticate<T>(HttpContext context, bool requireSecureConnection, Func<HttpContext, string, T> authorizationParser, TesterFunc<HttpContext, T, ConditionalValue<ClaimsPrincipal>, bool> principalParser, out ConditionalValue<ClaimsPrincipal> principal)
    {
        principal = null;
        try
        {
            principal = Authenticate(context, requireSecureConnection, authorizationParser, principalParser);
            return principal.Succeeded;
        }
        catch (Exception ex)
        {
            if (principal?.Failure != null) { ex = new AggregateException(ex, principal.Failure); }
            principal = new UnsuccessfulValue<ClaimsPrincipal>(ex);
            return false;
        }
    }

    /// <summary>
    /// Provides a generic way to make authentication requests using the specified <paramref name="context"/>.
    /// </summary>
    /// <typeparam name="T">The type of the credentials returned from <paramref name="authorizationParser"/> and passed to <paramref name="principalParser"/>.</typeparam>
    /// <param name="context">The context of the ASP.NET application.</param>
    /// <param name="requireSecureConnection">When <c>true</c>, the HTTP connection is required to use secure sockets (that is, HTTPS); when <c>false</c> no requirement is enforced.</param>
    /// <param name="authorizationParser">The function delegate that will parse the authorization header of a web request and return the credentials of <typeparamref name="T"/>.</param>
    /// <param name="principalParser">The function delegate that will parse the credentials of <typeparamref name="T"/> returned from <paramref name="authorizationParser"/> and if successful returns a <see cref="ClaimsPrincipal"/> object.</param>
    /// <returns>A <seealso cref="ClaimsPrincipal"/> if <paramref name="principalParser"/> was successful.</returns>
    /// <exception cref="SecurityException">
    /// Authorized failed for the request.
    /// </exception>
    public static ConditionalValue<ClaimsPrincipal> Authenticate<T>(HttpContext context, bool requireSecureConnection, Func<HttpContext, string, T> authorizationParser, TesterFunc<HttpContext, T, ConditionalValue<ClaimsPrincipal>, bool> principalParser)
    {
        Validator.ThrowIfNull(context);

        if (requireSecureConnection && !context.Request.IsHttps) { return new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException("An SSL connection is required for the request.")); }

        string authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException($"{HeaderNames.Authorization} header missing."));
        }

        var credentials = authorizationParser(context, authorizationHeader);
        if (credentials != null)
        {
            principalParser(context, credentials, out var principal);
            return principal; // can be either successful or unsuccessful
        }

        return new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException("Invalid credentials."));
    }
}
