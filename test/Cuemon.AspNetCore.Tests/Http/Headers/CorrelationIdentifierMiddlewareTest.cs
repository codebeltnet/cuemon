using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class CorrelationIdentifierMiddlewareTest : WebHostTest<ManagedWebHostFixture>
{
    private readonly IServiceProvider _provider;
    private readonly IApplicationBuilder _pipeline;

    public CorrelationIdentifierMiddlewareTest(ManagedWebHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _pipeline = hostFixture.Application;
        _provider = hostFixture.Host.Services;
    }

    [Fact]
    public async Task InvokeAsync_ShouldCreateNewCorrelationIdHeader_ConfiguredByDefault()
    {
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var options = _provider.GetRequiredService<IOptions<CorrelationIdentifierOptions>>();
        var pipeline = _pipeline.Build();

        await pipeline(context);

        Assert.True(context.Response.Headers.TryGetValue(options.Value.HeaderName, out var xCorrelationIdHeader));
        Assert.True(ParserFactory.FromGuid().TryParse(xCorrelationIdHeader.Single(), out var correlationId, o => o.Formats = GuidFormats.N));

        TestOutput.WriteLine(correlationId.ToString("N"));
    }

    [Fact]
    public async Task InvokeAsync_ShouldRelayCorrelationIdHeader_ConfiguredByDefault()
    {
        var expected = "072a8d5aa1cc4a16bf04132482748243";
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var options = _provider.GetRequiredService<IOptions<CorrelationIdentifierOptions>>();
        var pipeline = _pipeline.Build();

        context.Request.Headers.Add(options.Value.HeaderName, expected);

        await pipeline(context);

        Assert.True(context.Response.Headers.TryGetValue(options.Value.HeaderName, out var xCorrelationIdHeader));
        Assert.True(ParserFactory.FromGuid().TryParse(xCorrelationIdHeader.Single(), out var correlationId, o => o.Formats = GuidFormats.N));
        Assert.Equal(expected, correlationId.ToString("N"));

        TestOutput.WriteLine(expected);
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
        app.UseCorrelationIdentifier();
    }
}
