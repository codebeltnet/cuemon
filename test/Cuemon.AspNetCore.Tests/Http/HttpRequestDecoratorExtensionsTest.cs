using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class HttpRequestDecoratorExtensionsTest : Test
{
    public HttpRequestDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IsGetOrHeadMethod_ShouldRecognizeSupportedMethods()
    {
        var getContext = new DefaultHttpContext();
        getContext.Request.Method = HttpMethods.Get;
        var headContext = new DefaultHttpContext();
        headContext.Request.Method = HttpMethods.Head;
        var postContext = new DefaultHttpContext();
        postContext.Request.Method = HttpMethods.Post;

        Assert.True(Decorator.Enclose(getContext.Request).IsGetOrHeadMethod());
        Assert.True(Decorator.Enclose(headContext.Request).IsGetOrHeadMethod());
        Assert.False(Decorator.Enclose(postContext.Request).IsGetOrHeadMethod());
    }

    [Fact]
    public void IsClientSideResourceCached_ShouldRecognizeMatchingEntityTag()
    {
        var context = new DefaultHttpContext();
        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
        var entityTag = string.Concat("\"", builder.Checksum.ToHexadecimalString(), "\"");

        context.Request.Headers[HeaderNames.IfNoneMatch] = entityTag;

        Assert.True(Decorator.Enclose(context.Request).IsClientSideResourceCached(builder));
    }

    [Fact]
    public void IsClientSideResourceCached_ShouldReturnFalse_WhenEntityTagHeaderIsMissing()
    {
        var context = new DefaultHttpContext();
        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());

        Assert.False(Decorator.Enclose(context.Request).IsClientSideResourceCached(builder));
    }

    [Fact]
    public void IsClientSideResourceCached_ShouldRecognizeIfModifiedSinceHeader()
    {
        var context = new DefaultHttpContext();
        var lastModified = new DateTime(2024, 12, 24, 10, 11, 12, DateTimeKind.Utc);

        context.Request.Headers[HeaderNames.IfModifiedSince] = lastModified.ToString("R");

        Assert.True(Decorator.Enclose(context.Request).IsClientSideResourceCached(lastModified.AddMilliseconds(900)));
        Assert.False(Decorator.Enclose(context.Request).IsClientSideResourceCached(lastModified.AddSeconds(1)));
    }
}
