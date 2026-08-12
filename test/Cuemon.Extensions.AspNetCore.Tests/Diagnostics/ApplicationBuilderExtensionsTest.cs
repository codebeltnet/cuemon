using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Diagnostics;
public class ApplicationBuilderExtensionsTest : Test
{
    public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task UseServerTiming_ShouldWriteServerTimingHeader()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services => services.AddServerTiming(),
            app =>
            {
                app.Use(async (context, next) =>
                {
                    context.RequestServices.GetRequiredService<IServerTiming>().AddServerTiming("db", TimeSpan.FromMilliseconds(12));
                    await next();
                });
                app.UseServerTiming();
                app.Run(context => context.Response.WriteAsync("ok"));
            });

        var header = Assert.Single(response.Headers.GetValues(ServerTiming.HeaderName));

        Assert.StartsWith("db;dur=", header, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UseFaultDescriptorExceptionHandler_ShouldSerializeHttpExceptionDescriptor_AsJson()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions();
                services.AddJsonExceptionResponseFormatter();
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Run(_ => throw new NotFoundException());
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        Assert.Contains("\"status\": 404", body, StringComparison.Ordinal);
        Assert.Contains("\"code\": \"NotFound\"", body, StringComparison.Ordinal);
    }
}
