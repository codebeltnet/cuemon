using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Messaging;
using Cuemon.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class RequestIdentifierMiddlewareTest : WebHostTest<ManagedWebHostFixture>
{
    private readonly IServiceProvider _provider;
    private readonly IApplicationBuilder _pipeline;

    public RequestIdentifierMiddlewareTest(ManagedWebHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _pipeline = hostFixture.Application;
        _provider = hostFixture.Host.Services;
    }

    [Fact]
    public async Task InvokeAsync_ShouldCreateNewRequestIdHeader_ConfiguredByDelegate()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var options = _provider.GetRequiredService<IOptions<RequestIdentifierOptions>>();
        var pipeline = _pipeline.Build();

        await pipeline(context);

        Assert.True(context.Response.Headers.TryGetValue(options.Value.HeaderName, out var xRequestIdHeader));

        var requestId = xRequestIdHeader.Single();

        Assert.False(ParserFactory.FromGuid().TryParse(requestId, out _, o => o.Formats = GuidFormats.N));
        Assert.Equal(32, requestId.Length);

        TestOutput.WriteLine(requestId);
    }

    [Fact]
    public async Task InvokeAsync_ShouldIgnoreExistingRequestIdHeader_ConfiguredByDefault()
    {
        var expected = "072a8d5aa1cc4a16bf04132482748243";
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var options = _provider.GetRequiredService<IOptions<RequestIdentifierOptions>>();
        var pipeline = _pipeline.Build();

        context.Request.Headers.Add(options.Value.HeaderName, expected);

        await pipeline(context);

        Assert.True(context.Response.Headers.TryGetValue(options.Value.HeaderName, out var xRequestIdHeader));

        var requestId = xRequestIdHeader.Single();

        Assert.False(ParserFactory.FromGuid().TryParse(requestId, out _, o => o.Formats = GuidFormats.N));
        Assert.NotEqual(expected, requestId);
        Assert.Equal(32, requestId.Length);

        TestOutput.WriteLine(requestId);
    }

    /// <summary>
    /// Adds services to the container.
    /// </summary>
    /// <param name="services">The collection of service descriptors.</param>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddFakeHttpContextAccessor(ServiceLifetime.Transient);
    }

    public override void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseRequestIdentifier(o => o.Token = new RequestToken(Generate.RandomString(32, Alphanumeric.PunctuationMarks, Alphanumeric.Numbers)));
    }
}
