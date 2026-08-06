using System;
using System.Security;
using System.Security.Claims;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class AuthenticatorTest : Test
{
    public AuthenticatorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Authenticate_ShouldFail_WhenSecureConnectionIsRequired()
    {
        var context = new DefaultHttpContext();

        var result = Authenticator.Authenticate(context, true, (_, authorization) => authorization, PrincipalParserSuccess);

        Assert.False(result.Succeeded);
        Assert.Equal("An SSL connection is required for the request.", result.Failure.Message);
    }

    [Fact]
    public void Authenticate_ShouldFail_WhenAuthorizationHeaderIsMissing()
    {
        var context = new DefaultHttpContext();
        context.Request.IsHttps = true;

        var result = Authenticator.Authenticate(context, true, (_, authorization) => authorization, PrincipalParserSuccess);

        Assert.False(result.Succeeded);
        Assert.Equal("Authorization header missing.", result.Failure.Message);
    }

    [Fact]
    public void Authenticate_ShouldFail_WhenAuthorizationParserReturnsNull()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Append(HeaderNames.Authorization, "ignored");

        var result = Authenticator.Authenticate<string>(context, false, (_, _) => null, PrincipalParserSuccess);

        Assert.False(result.Succeeded);
        Assert.Equal("Invalid credentials.", result.Failure.Message);
    }

    [Fact]
    public void Authenticate_ShouldReturnPrincipal_WhenPrincipalParserSucceeds()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Append(HeaderNames.Authorization, "token");

        var result = Authenticator.Authenticate(context, false, (_, authorization) => authorization, PrincipalParserSuccess);

        Assert.True(result.Succeeded);
        Assert.Equal("Agent", result.Result.Identity.Name);
    }

    [Fact]
    public void TryAuthenticate_ShouldCaptureThrownExceptions()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Append(HeaderNames.Authorization, "token");

        var succeeded = Authenticator.TryAuthenticate(context, false, (_, authorization) => authorization, PrincipalParserThrowing, out var principal);

        Assert.False(succeeded);
        var failure = Assert.IsType<InvalidOperationException>(principal.Failure);
        Assert.Equal("outer", failure.Message);
    }

    private static bool PrincipalParserSuccess(HttpContext context, string credentials, out ConditionalValue<ClaimsPrincipal> principal)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Agent") }, "scheme");
        principal = new SuccessfulValue<ClaimsPrincipal>(new ClaimsPrincipal(identity));
        return true;
    }

    private static bool PrincipalParserThrowing(HttpContext context, string credentials, out ConditionalValue<ClaimsPrincipal> principal)
    {
        principal = new UnsuccessfulValue<ClaimsPrincipal>(new SecurityException("inner"));
        throw new InvalidOperationException("outer");
    }
}
