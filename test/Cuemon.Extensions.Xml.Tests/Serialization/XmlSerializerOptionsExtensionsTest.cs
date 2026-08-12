using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions.Xml.Serialization;
public class XmlSerializerOptionsExtensionsTest : Test
{
    public XmlSerializerOptionsExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void XmlSerializerOptionsExtensions_ShouldApplyDefaultSettings()
    {
        var previous = XmlConvert.DefaultSettings;
        var options = new XmlSerializerOptions();
        try
        {
            options.ApplyToDefaultSettings();

            Assert.Same(options, XmlConvert.DefaultSettings());
            Assert.Throws<ArgumentNullException>(() => XmlSerializerOptionsExtensions.ApplyToDefaultSettings(null));
        }
        finally
        {
            XmlConvert.DefaultSettings = previous;
        }
    }
}
