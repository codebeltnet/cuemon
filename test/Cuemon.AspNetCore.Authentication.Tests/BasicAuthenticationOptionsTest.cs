using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class BasicAuthenticationOptionsTest : Test
{
    public BasicAuthenticationOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void BasicAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticatorIsNull()
    {
        var sut1 = new BasicAuthenticationOptions();

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Authenticator == null')", sut2.Message);
        Assert.Equal("BasicAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void BasicAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmIsNull()
    {
        var sut1 = new BasicAuthenticationOptions
        {
            Authenticator = (username, password) => ClaimsPrincipal.Current,
            Realm = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("BasicAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void BasicAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmIsEmpty()
    {
        var sut1 = new BasicAuthenticationOptions
        {
            Authenticator = (username, password) => ClaimsPrincipal.Current,
            Realm = ""
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("BasicAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void BasicAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmHasWhitespace()
    {
        var sut1 = new BasicAuthenticationOptions
        {
            Authenticator = (username, password) => ClaimsPrincipal.Current,
            Realm = " "
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("BasicAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void BasicAuthenticationOptions_ShouldHaveDefaultValues()
    {
        var sut = new BasicAuthenticationOptions();

        var realm = "AuthenticationServer";

        Assert.Equal(realm, sut.Realm);
        Assert.Null(sut.Authenticator);
    }
}
