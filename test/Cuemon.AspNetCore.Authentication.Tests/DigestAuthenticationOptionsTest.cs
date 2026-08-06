using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Digest;
using Codebelt.Extensions.Xunit;
using Cuemon.Security.Cryptography;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class DigestAuthenticationOptionsTest : Test
{
    public DigestAuthenticationOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenAuthenticatorIsNull()
    {
        var sut1 = new DigestAuthenticationOptions();

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Authenticator == null')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenNonceExpiredParserIsNull()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            NonceExpiredParser = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NonceExpiredParser == null')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenNonceGeneratorIsNull()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            NonceGenerator = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NonceGenerator == null')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenNonceSecretIsNull()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            NonceSecret = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NonceSecret == null')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenOpaqueGeneratorIsNull()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            OpaqueGenerator = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'OpaqueGenerator == null')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmIsNull()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            Realm = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmIsEmpty()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            Realm = ""
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldThrowInvalidOperationException_WhenRealmHasWhitespace()
    {
        var sut1 = new DigestAuthenticationOptions
        {
            Authenticator = (string username, out string password) =>
            {
                password = "";
                return ClaimsPrincipal.Current;
            },
            Realm = " "
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'string.IsNullOrWhiteSpace(Realm)')", sut2.Message);
        Assert.Equal("DigestAuthenticationOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void DigestAuthenticationOptions_ShouldHaveDefaultValues()
    {
        var sut = new DigestAuthenticationOptions();

        Assert.Equal(DigestCryptoAlgorithm.Sha256, sut.DigestAlgorithm);
        Assert.NotNull(sut.OpaqueGenerator);
        Assert.NotNull(sut.NonceExpiredParser);
        Assert.NotNull(sut.NonceGenerator);
        Assert.NotNull(sut.NonceSecret);
        Assert.Equal("AuthenticationServer", sut.Realm);
        Assert.Null(sut.Authenticator);
    }
}
