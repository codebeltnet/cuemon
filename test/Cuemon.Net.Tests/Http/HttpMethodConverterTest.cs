using System;
using System.Net.Http;
using Codebelt.Extensions.Xunit;
using Cuemon.Net.Http;
using Xunit;

namespace Cuemon.Net.Http;
/// <summary>
/// Tests for the <see cref="HttpMethodConverter"/> class and <see cref="HttpRequestOptions"/>.
/// </summary>
public class HttpMethodConverterTest : Test
{
    public HttpMethodConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HttpMethodConverterAndRequestOptions_ShouldResolveKnownValues()
    {
        Assert.Equal(HttpMethods.Get, HttpMethodConverter.ToHttpMethod(HttpMethod.Get));
        Assert.Equal(HttpMethods.Post, HttpMethodConverter.ToHttpMethod(HttpMethod.Post));
        Assert.Equal(HttpMethods.Put, HttpMethodConverter.ToHttpMethod(HttpMethod.Put));
        Assert.Equal(HttpMethods.Delete, HttpMethodConverter.ToHttpMethod(HttpMethod.Delete));
        Assert.Equal(HttpMethods.Head, HttpMethodConverter.ToHttpMethod(HttpMethod.Head));
        Assert.Equal(HttpMethods.Options, HttpMethodConverter.ToHttpMethod(HttpMethod.Options));
        Assert.Equal(HttpMethods.Trace, HttpMethodConverter.ToHttpMethod(HttpMethod.Trace));
        Assert.Equal(HttpMethods.Patch, HttpMethodConverter.ToHttpMethod(new HttpMethod("PATCH")));
        Assert.Equal(HttpMethods.Get, HttpMethodConverter.ToHttpMethod(new HttpMethod("CUSTOM")));
        Assert.Throws<ArgumentNullException>(() => HttpMethodConverter.ToHttpMethod(null));

        var options = new HttpRequestOptions();
        Assert.NotNull(options.Request);
        Assert.Equal(HttpCompletionOption.ResponseContentRead, options.CompletionOption);
        options.Request.Method = HttpMethod.Head;
        Assert.Equal(HttpCompletionOption.ResponseHeadersRead, options.CompletionOption);
        options.Request.Method = HttpMethod.Trace;
        Assert.Equal(HttpCompletionOption.ResponseHeadersRead, options.CompletionOption);
    }
}
