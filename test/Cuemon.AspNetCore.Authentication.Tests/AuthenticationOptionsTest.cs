using System;
using System.Net.Http;
using System.Net;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class AuthenticationOptionsTest : Test
{
    public AuthenticationOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AuthenticationOptions_ShouldThrowInvalidOperationException_WhenUnauthorizedMessageIsNull()
    {
        var sut1 = new FakeAuthenticationOptions
        {
            UnauthorizedMessage = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'UnauthorizedMessage == null')", sut2.Message);
        Assert.Equal("FakeAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void AuthenticationOptions_ShouldHaveDefaultValues()
    {
        var sut = new FakeAuthenticationOptions();

        var unathorizedMessage = "The request has not been applied because it lacks valid authentication credentials for the target resource.";
        var responseHandlder = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent(unathorizedMessage) });

        Assert.Equal(unathorizedMessage, sut.UnauthorizedMessage);
        Assert.Equivalent(responseHandlder.Invoke(), sut.ResponseHandler.Invoke(), true);
        Assert.True(sut.RequireSecureConnection);
    }

    private class FakeAuthenticationOptions : AuthenticationOptions
    {
    }
}
