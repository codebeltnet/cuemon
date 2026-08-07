using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http;
public class HttpResponseExtensionsTest : WebHostTest<ManagedWebHostFixture>
{
    private readonly IServiceProvider _provider;
    private readonly IApplicationBuilder _pipeline;

    public HttpResponseExtensionsTest(ManagedWebHostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
    {
        _pipeline = hostFixture.Application;
        _provider = hostFixture.Host.Services;
    }

    [Fact]
    public async Task AddOrUpdateEntityTagHeader_ShouldHaveNoEntityTagHeader()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        Assert.Empty(context.Response.Headers[HeaderNames.ETag].ToString());
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task AddOrUpdateEntityTagHeader_ShouldAddEntityTagHeader()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        context.Response.AddOrUpdateEntityTagHeader(context.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Collection(context.Response.Headers[HeaderNames.ETag], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
    }

    [Fact]
    public async Task AddOrUpdateEntityTagHeader_ShouldAddEntityTagHeader_ChangingStatusCodeToNotModified()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        context.Request.Headers.Add(HeaderNames.IfNoneMatch, "\"648e5c529d659a6882387b946cc37a83\"");
        context.Response.AddOrUpdateEntityTagHeader(context.Request, new ChecksumBuilder(() => HashFactory.CreateFnv128()));

        Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
        Assert.Collection(context.Request.Headers[HeaderNames.IfNoneMatch], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
        Assert.Collection(context.Response.Headers[HeaderNames.ETag], s => Assert.Equal("\"648e5c529d659a6882387b946cc37a83\"", s));
    }

    [Fact]
    public async Task AddOrUpdateLastModifiedHeader_ShouldHaveNoLastModifiedHeader()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        Assert.Empty(context.Response.Headers[HeaderNames.LastModified].ToString());
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task AddOrUpdateLastModifiedHeader_ShouldAddLastModifiedHeader()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        context.Response.AddOrUpdateLastModifiedHeader(context.Request, DateTime.UnixEpoch);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Collection(context.Response.Headers[HeaderNames.LastModified], s => Assert.Equal("Thu, 01 Jan 1970 00:00:00 GMT", s));
    }

    [Fact]
    public async Task AddOrUpdateLastModifiedHeader_ShouldAddLastModifiedHeader_ChangingStatusCodeToNotModified()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var pipeline = _pipeline.Build();

        await pipeline(context);

        context.Request.Headers.Add(HeaderNames.IfModifiedSince, DateTime.UnixEpoch.ToString("R"));
        context.Response.AddOrUpdateLastModifiedHeader(context.Request, DateTime.UnixEpoch);

        Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
        Assert.Collection(context.Request.Headers[HeaderNames.IfModifiedSince], s => Assert.Equal("Thu, 01 Jan 1970 00:00:00 GMT", s));
        Assert.Collection(context.Response.Headers[HeaderNames.LastModified], s => Assert.Equal("Thu, 01 Jan 1970 00:00:00 GMT", s));
    }

    [Fact]
    public async Task WriteBodyAsync_ShouldWriteTextToResponseBody()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;

        await context.Response.WriteBodyAsync(() => "The quick brown fox jumps over the lazy dog.".ToByteArray());

        Assert.Equal("The quick brown fox jumps over the lazy dog.", context.Response.Body.ToEncodedString());
    }

    [Fact]
    public void OnStartingInvokeTransformer_ShouldTransferResponseMessageToResponse()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var sut = new HttpResponseMessage(HttpStatusCode.EarlyHints)
        {
            Content = new StringContent("The quick brown fox jumps over the lazy dog.")
        };

        context.Response.OnStartingInvokeTransformer(sut, async (message, response) =>
        {
            response.StatusCode = (int)message.StatusCode;
            var content = await message.Content.ReadAsByteArrayAsync();
            await response.WriteBodyAsync(() => content);
        });

        Assert.Equal(103, context.Response.StatusCode);
        Assert.Equal("The quick brown fox jumps over the lazy dog.", context.Response.Body.ToEncodedString());
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddTransient<IHttpContextAccessor, FakeHttpContextAccessor>();
    }

    public override void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(routes => routes.MapGet("/", context =>
        {
            context.Response.StatusCode = 200;
            return Task.CompletedTask;
        }));
    }
}
