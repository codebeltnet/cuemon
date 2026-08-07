using Codebelt.Extensions.Xunit;
using System;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class AuthorizationHeaderOptionsTest : Test
{
    public AuthorizationHeaderOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AuthorizationHeaderOptions_ShouldThrowInvalidOperationException_ForCredentialsDelimiterBeingNull()
    {
        var sut1 = new AuthorizationHeaderOptions
        {
            CredentialsDelimiter = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'CredentialsDelimiter == null')", sut2.Message);
        Assert.Equal("AuthorizationHeaderOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void AuthorizationHeaderOptions_ShouldThrowInvalidOperationException_ForCredentialsKeyValueDelimiterBeingNull()
    {
        var sut1 = new AuthorizationHeaderOptions
        {
            CredentialsKeyValueDelimiter = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'CredentialsKeyValueDelimiter == null')", sut2.Message);
        Assert.Equal("AuthorizationHeaderOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void AuthorizationHeaderOptions_ShouldHaveDefaultValues()
    {
        var sut = new AuthorizationHeaderOptions();

        Assert.Equal("=", sut.CredentialsKeyValueDelimiter);
        Assert.Equal(",", sut.CredentialsDelimiter);
    }
}
