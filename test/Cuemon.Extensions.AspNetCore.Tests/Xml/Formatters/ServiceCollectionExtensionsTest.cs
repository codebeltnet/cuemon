using System.Linq;
using Cuemon.Xml.Serialization.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Xml.Formatters;
public class ServiceCollectionExtensionsTest : Test
{
    public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddXmlFormatterOptions_ShouldOnlyRegisterOnce_WhenCalledMultipleTimes()
    {
        var sut = new ServiceCollection();

        sut.AddXmlFormatterOptions();
        sut.AddXmlFormatterOptions();
        sut.AddXmlFormatterOptions();

        var configureOptionsCount = sut.Count(sd =>
            sd.ServiceType == typeof(IConfigureOptions<XmlFormatterOptions>));

        TestOutput.WriteLine($"IConfigureOptions<XmlFormatterOptions> registrations: {configureOptionsCount}");

        Assert.Equal(1, configureOptionsCount);
    }
}
