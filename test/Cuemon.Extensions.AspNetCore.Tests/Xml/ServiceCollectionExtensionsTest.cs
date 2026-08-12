using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Xml.Serialization.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Xml;
public class ServiceCollectionExtensionsTest : Test
{
    public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddMinimalXmlOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddMinimalXmlOptions(null));
    }

    [Fact]
    public void AddMinimalXmlOptions_ShouldRegisterXmlFormatterOptions()
    {
        var sut = new ServiceCollection();

        sut.AddMinimalXmlOptions();

        var count = sut.Count(sd =>
            sd.ServiceType == typeof(IConfigureOptions<XmlFormatterOptions>));

        TestOutput.WriteLine($"IConfigureOptions<XmlFormatterOptions> registrations: {count}");

        Assert.True(count >= 1);
    }

    [Fact]
    public void AddMinimalXmlOptions_ShouldAlsoRegisterXmlExceptionResponseFormatter()
    {
        var sut = new ServiceCollection();
        sut.AddFaultDescriptorOptions();

        sut.AddMinimalXmlOptions();

        var hasFormatter = sut.Any(sd =>
            sd.ServiceType == typeof(HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>));

        Assert.True(hasFormatter);
    }

    [Fact]
    public void AddMinimalXmlOptions_ShouldOnlyRegisterXmlExceptionResponseFormatterOnce_WhenCalledMultipleTimes()
    {
        var sut = new ServiceCollection();
        sut.AddFaultDescriptorOptions();

        sut.AddMinimalXmlOptions();
        sut.AddMinimalXmlOptions();
        sut.AddMinimalXmlOptions();

        var count = sut.Count(sd =>
            sd.ServiceType == typeof(HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>));

        TestOutput.WriteLine($"HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions> registrations: {count}");

        Assert.Equal(1, count);
    }
}
