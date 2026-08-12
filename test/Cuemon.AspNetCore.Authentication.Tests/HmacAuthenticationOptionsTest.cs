using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;
using Codebelt.Extensions.Xunit;
using Cuemon.Security.Cryptography;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class HmacAuthenticationOptionsTest : Test
{
    public HmacAuthenticationOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HmacAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticatorIsNull()
    {
        var sut1 = new HmacAuthenticationOptions();

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Authenticator == null')", sut2.Message);
        Assert.Equal("HmacAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HmacAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticationSchemeIsNull()
    {
        var sut1 = new HmacAuthenticationOptions
        {
            Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = null;
                return ClaimsPrincipal.Current;
            },
            AuthenticationScheme = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(AuthenticationScheme)')", sut2.Message);
        Assert.Equal("HmacAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HmacAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticationSchemeIsEmpty()
    {
        var sut1 = new HmacAuthenticationOptions
        {
            Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = null;
                return ClaimsPrincipal.Current;
            },
            AuthenticationScheme = ""
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(AuthenticationScheme)')", sut2.Message);
        Assert.Equal("HmacAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HmacAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticationSchemeHasWhitespace()
    {
        var sut1 = new HmacAuthenticationOptions
        {
            Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = null;
                return ClaimsPrincipal.Current;
            },
            AuthenticationScheme = " "
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(AuthenticationScheme)')", sut2.Message);
        Assert.Equal("HmacAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HmacAuthenticationOptions_ShouldHaveDefaultValues()
    {
        var sut = new HmacAuthenticationOptions();

        Assert.Equal(HmacFields.Scheme, sut.AuthenticationScheme);
        Assert.Equal(KeyedCryptoAlgorithm.HmacSha256, sut.Algorithm);
        Assert.Null(sut.Authenticator);
    }
}
