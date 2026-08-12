using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class HttpExceptionDescriptorResponseHandlerOptionsTest : Test
{
    public HttpExceptionDescriptorResponseHandlerOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HttpExceptionDescriptorResponseHandlerOptions_ContentFactoryIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HttpExceptionDescriptorResponseHandlerOptions();
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ContentFactory == null')", sut2.Message);
        Assert.Equal("HttpExceptionDescriptorResponseHandlerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpExceptionDescriptorResponseHandlerOptions_ContentTypeIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HttpExceptionDescriptorResponseHandlerOptions
        {
            ContentFactory = descriptor => new StringContent("text/plain"),
            StatusCodeFactory = descriptor => HttpStatusCode.Continue
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ContentType == null')", sut2.Message);
        Assert.Equal("HttpExceptionDescriptorResponseHandlerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpExceptionDescriptorResponseHandlerOptions_StatusCodeFactoryIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HttpExceptionDescriptorResponseHandlerOptions
        {
            ContentType = new MediaTypeHeaderValue("text/plain"),
            ContentFactory = descriptor => new StringContent("text/plain")
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'StatusCodeFactory == null')", sut2.Message);
        Assert.Equal("HttpExceptionDescriptorResponseHandlerOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpExceptionDescriptorResponseHandlerOptions_ShouldHaveDefaultValues()
    {
        var sut = new HttpExceptionDescriptorResponseHandlerOptions();

        Assert.Null(sut.ContentFactory);
        Assert.Null(sut.ContentType);
        Assert.Null(sut.StatusCodeFactory);
    }
}
