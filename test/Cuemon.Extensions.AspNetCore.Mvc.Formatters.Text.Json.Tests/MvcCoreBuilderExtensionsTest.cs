using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Assets;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;

public class MvcCoreBuilderExtensionsTest : Test
{
    public MvcCoreBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddJsonFormatters_ShouldRegisterFormatters_WhenCalledOnIMvcCoreBuilder()
    {
        using var filter = WebHostTestFactory.Create(services =>
        {
            services.AddMvcCore()
                .AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, app =>
        {
            app.UseRouting();
            app.UseEndpoints(routes => { routes.MapControllers(); });
        });

        var client = filter.Host.GetTestClient();
        var result = await client.GetAsync("/fake");
        var model = await result.Content.ReadAsStringAsync();

        TestOutput.WriteLine(model);

        Assert.Contains("\"date\":", model);
        Assert.Contains("\"temperatureC\":", model);
        Assert.Contains("\"temperatureF\":", model);
        Assert.Contains("\"summary\":", model);

        Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        Assert.Equal(HttpMethod.Get, result.RequestMessage.Method);
    }

    [Fact]
    public void AddJsonFormattersOptions_ShouldConfigureOptions_WhenCalledOnIMvcCoreBuilder()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvcCore();

        var result = builder.AddJsonFormattersOptions(o => o.Settings.WriteIndented = true);

        Assert.Same(builder, result);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;

        Assert.True(options.Settings.WriteIndented);
    }
}
