using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class SeeOtherResultTest : HostTest<ManagedHostFixture>
{
    private readonly IServiceProvider _provider;

    public SeeOtherResultTest(ManagedHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _provider = hostFixture.Host.Services;
    }

    [Fact]
    public async Task ExecuteResultAsync_ShouldReturnStatusCode303AndLocationUri()
    {
        var uri = new Uri("https://www.cuemon.net/");
        var context = _provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var sor = new SeeOtherResult(uri);
        var ac = new ActionContext(context, new RouteData(), new ActionDescriptor());

        Assert.Equal(StatusCodes.Status303SeeOther, sor.StatusCode);
        Assert.Equal(uri, sor.Location);

        await sor.ExecuteResultAsync(ac);

        Assert.Equal(sor.StatusCode, context.Response.StatusCode);
        Assert.Equal(sor.Location.OriginalString, context.Response.Headers[HeaderNames.Location]);
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddFakeHttpContextAccessor(ServiceLifetime.Transient);
    }
}
