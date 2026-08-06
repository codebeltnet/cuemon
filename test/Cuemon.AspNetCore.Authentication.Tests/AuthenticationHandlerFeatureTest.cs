using System.Security.Claims;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class AuthenticationHandlerFeatureTest : Test
{
    public AuthenticationHandlerFeatureTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Set_ShouldPropagateAuthenticateResultAndUserToHttpFeatures()
    {
        var context = new DefaultHttpContext();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Agent") }, "scheme"));
        var result = AuthenticateResult.Success(new AuthenticationTicket(principal, "scheme"));

        AuthenticationHandlerFeature.Set(result, context);

        var authenticateFeature = Assert.IsType<AuthenticationHandlerFeature>(context.Features.Get<IAuthenticateResultFeature>());
        var httpAuthenticationFeature = Assert.IsType<AuthenticationHandlerFeature>(context.Features.Get<IHttpAuthenticationFeature>());

        Assert.Same(authenticateFeature, httpAuthenticationFeature);
        Assert.Same(result, authenticateFeature.AuthenticateResult);
        Assert.Same(principal, authenticateFeature.User);
    }

    [Fact]
    public void UserSetter_ShouldClearAuthenticateResult()
    {
        var result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), "scheme"));
        var sut = new AuthenticationHandlerFeature(result);
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        sut.User = principal;

        Assert.Same(principal, sut.User);
        Assert.Null(sut.AuthenticateResult);
    }
}
