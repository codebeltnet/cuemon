using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Assets;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
public class XmlSerializationInputFormatterTest : Test
{
    public XmlSerializationInputFormatterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_VerifyThatUtf8AndUtf16_WasAdded_ToSupportedEncodings()
    {
        var sut = new XmlSerializationInputFormatter(new XmlFormatterOptions());

        Assert.Equal(2, sut.SupportedEncodings.Count);
        Assert.Collection(sut.SupportedEncodings,
            e => Assert.Equal(Encoding.UTF8, e),
            e => Assert.Equal(Encoding.Unicode, e));
    }

    [Fact]
    public void Ctor_VerifyThatApplicationXmlAndTextXml_WasAdded_ToSupportedMediaTypes()
    {
        var sut = new XmlSerializationInputFormatter(new XmlFormatterOptions());

        Assert.Equal(3, sut.SupportedMediaTypes.Count);
        Assert.Collection(sut.SupportedMediaTypes,
            s => Assert.Contains("application/xml", s),
            s => Assert.Contains("text/xml", s),
            s => Assert.Contains("application/problem+xml", s));
    }

    [Fact]
    public async Task ReadRequestBodyAsync_ShouldReturnCreated()
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
            var wf = new WeatherForecast();
            var formatter = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var stream = formatter.Serialize(wf);
            var client = filter.Host.GetTestClient();

            var result = await client.PostAsync("/fake", new StringContent(stream.ToEncodedString(), Encoding.UTF8, "application/xml"));
            var model = await result.Content.ReadAsStringAsync();

            Assert.Contains("<WeatherForecast>", model);
            Assert.Contains($"<Date>{wf.Date.ToString("O", CultureInfo.InvariantCulture)}", model);
            Assert.Contains($"<TemperatureC>{wf.TemperatureC}", model);
            Assert.Contains($"<TemperatureF>{wf.TemperatureF}", model);
            Assert.Contains($"<Summary>{wf.Summary}", model);

            Assert.Equal(StatusCodes.Status201Created, (int)result.StatusCode);
            Assert.Equal(HttpMethod.Post, result.RequestMessage.Method);
            Assert.Equal(new Uri("http://localhost/fake"), result.RequestMessage.RequestUri);
        }
    }
}
