using System;
using System.Net;
using Codebelt.Extensions.Xunit;
using Cuemon.Net.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class ApiKeySentinelOptionsTest : Test
{
    public ApiKeySentinelOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ApiKeySentinelOptions_HeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            HeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_HeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            HeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_HeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            HeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_ResponseHandlerIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            ResponseHandler = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ResponseHandler == null')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_AllowedKeysIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            AllowedKeys = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'AllowedKeys == null')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_GenericClientStatusCodeIsOutOfRange_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ApiKeySentinelOptions
        {
            GenericClientStatusCode = HttpStatusCode.Ambiguous
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        TestOutput.WriteLine(sut2.Message);


        Assert.Equal("Operation is not valid due to the current state of the object. (Expression '(int)GenericClientStatusCode < 400 || (int)GenericClientStatusCode > 499')", sut2.Message);
        Assert.Equal("ApiKeySentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ApiKeySentinelOptions_ShouldHaveDefaultValues()
    {
        var sut = new ApiKeySentinelOptions();

        Assert.NotNull(sut.AllowedKeys);
        Assert.Equal("The requirements of the request was not met.", sut.GenericClientMessage);
        Assert.Equal("The API key specified was rejected.", sut.ForbiddenMessage);
        Assert.Equal(HttpHeaderNames.XApiKey, sut.HeaderName);
        Assert.NotNull(sut.ResponseHandler);
        Assert.False(sut.UseGenericResponse);
    }
}
