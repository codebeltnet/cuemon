using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Net.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class RequestIdentifierOptionsTest : Test
{
    public RequestIdentifierOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void RequestIdentifierOptions_HeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new RequestIdentifierOptions
        {
            HeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("RequestIdentifierOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void RequestIdentifierOptions_HeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new RequestIdentifierOptions
        {
            HeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("RequestIdentifierOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void RequestIdentifierOptions_HeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new RequestIdentifierOptions
        {
            HeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("RequestIdentifierOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void RequestIdentifierOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new RequestIdentifierOptions()
        {
            Token = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Token == null')", sut2.Message);
        Assert.Equal("RequestIdentifierOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void RequestIdentifierOptions_ShouldHaveDefaultValues()
    {
        var sut = new RequestIdentifierOptions();

        Assert.NotNull(sut.Token);
        Assert.Equal(HttpHeaderNames.XRequestId, sut.HeaderName);
    }
}
