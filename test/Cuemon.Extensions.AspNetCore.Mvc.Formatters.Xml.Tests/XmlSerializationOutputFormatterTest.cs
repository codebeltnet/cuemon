using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Assets;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
public class XmlSerializationOutputFormatterTest : Test
{
    public XmlSerializationOutputFormatterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_VerifyThatUtf8AndUtf16_WasAdded_ToSupportedEncodings()
    {
        var sut = new XmlSerializationOutputFormatter(new XmlFormatterOptions());

        Assert.Equal(2, sut.SupportedEncodings.Count);
        Assert.Collection(sut.SupportedEncodings,
            e => Assert.Equal(Encoding.UTF8, e),
            e => Assert.Equal(Encoding.Unicode, e));
    }

    [Fact]
    public void Ctor_VerifyThatApplicationXmlAndTextXml_WasAdded_ToSupportedMediaTypes()
    {
        var sut = new XmlSerializationOutputFormatter(new XmlFormatterOptions());

        Assert.Equal(3, sut.SupportedMediaTypes.Count);
        Assert.Collection(sut.SupportedMediaTypes,
            s => Assert.Contains("application/xml", s),
            s => Assert.Contains("text/xml", s),
            s => Assert.Contains("application/problem+xml", s));
    }

    [Fact]
    public async Task WriteResponseBodyAsync_ShouldReturnOk()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); })
                .AddApplicationPart(typeof(FakeController).Assembly)
                .AddXmlFormatters();
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
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
    }
}
