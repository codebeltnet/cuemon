using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class HttpResponseDecoratorExtensionsTest : Test
{
    public HttpResponseDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddOrUpdateEntityTagHeader_ShouldSetNotModified_WhenClientCacheMatches()
    {
        var warmupContext = new DefaultHttpContext();
        warmupContext.Response.StatusCode = StatusCodes.Status200OK;

        Decorator.Enclose(warmupContext.Response).AddOrUpdateEntityTagHeader(warmupContext.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

        var expectedEntityTag = warmupContext.Response.Headers[HeaderNames.ETag].ToString();
        var context = new DefaultHttpContext();
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Request.Headers[HeaderNames.IfNoneMatch] = expectedEntityTag;

        Decorator.Enclose(context.Response).AddOrUpdateEntityTagHeader(context.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

        Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
        Assert.Equal(expectedEntityTag, context.Response.Headers[HeaderNames.ETag]);
    }

    [Fact]
    public void AddOrUpdateEntityTagHeader_ShouldAddWeakEntityTag_WhenRequested()
    {
        var context = new DefaultHttpContext();
        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());

        context.Response.StatusCode = StatusCodes.Status200OK;

        Decorator.Enclose(context.Response).AddOrUpdateEntityTagHeader(context.Request, builder, true);

        Assert.StartsWith("W/\"", context.Response.Headers[HeaderNames.ETag].ToString(), StringComparison.Ordinal);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public void AddOrUpdateLastModifiedHeader_ShouldSetNotModified_WhenClientCacheMatches()
    {
        var context = new DefaultHttpContext();
        var lastModified = new DateTime(2024, 12, 24, 10, 11, 12, DateTimeKind.Utc);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Request.Headers[HeaderNames.IfModifiedSince] = lastModified.ToString("R");

        Decorator.Enclose(context.Response).AddOrUpdateLastModifiedHeader(context.Request, lastModified);

        Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
        Assert.Equal(lastModified.ToString("R"), context.Response.Headers[HeaderNames.LastModified]);
    }

    [Fact]
    public void AddOrUpdateLastModifiedHeader_ShouldWriteHeader_WhenCacheDoesNotMatch()
    {
        var context = new DefaultHttpContext();
        var lastModified = new DateTime(2024, 12, 24, 10, 11, 12, DateTimeKind.Utc);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Request.Headers[HeaderNames.IfModifiedSince] = lastModified.AddSeconds(-2).ToString("R");

        Decorator.Enclose(context.Response).AddOrUpdateLastModifiedHeader(context.Request, lastModified);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Equal(lastModified.ToString("R"), context.Response.Headers[HeaderNames.LastModified]);
    }
}
