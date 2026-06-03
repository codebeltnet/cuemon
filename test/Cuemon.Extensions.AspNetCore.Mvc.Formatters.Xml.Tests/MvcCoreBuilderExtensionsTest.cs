using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Assets;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;

public class MvcCoreBuilderExtensionsTest : Test
{
    public MvcCoreBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddXmlFormatters_ShouldRegisterFormatters_WhenCalledOnIMvcCoreBuilder()
    {
        using var filter = WebHostTestFactory.Create(services =>
        {
            services.AddMvcCore()
                .AddApplicationPart(typeof(FakeController).Assembly)
                .AddXmlFormatters();
        }, app =>
        {
            app.UseRouting();
            app.UseEndpoints(routes => { routes.MapControllers(); });
        });

        var client = filter.Host.GetTestClient();
        var result = await client.GetAsync("/fake");
        var model = await result.Content.ReadAsStringAsync();

        TestOutput.WriteLine(model);

        Assert.Contains("<WeatherForecast>", model);
        Assert.Contains("<Date>", model);
        Assert.Contains("<TemperatureC>", model);
        Assert.Contains("<TemperatureF>", model);
        Assert.Contains("<Summary>", model);

        Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        Assert.Equal(HttpMethod.Get, result.RequestMessage.Method);
    }

    [Fact]
    public void AddXmlFormattersOptions_ShouldConfigureOptions_WhenCalledOnIMvcCoreBuilder()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvcCore();

        var result = builder.AddXmlFormattersOptions(o => o.SynchronizeWithXmlConvert = true);

        Assert.Same(builder, result);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;

        Assert.True(options.SynchronizeWithXmlConvert);
    }
}
