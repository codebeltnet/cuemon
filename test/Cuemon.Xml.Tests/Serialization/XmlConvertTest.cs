using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.Xml.Serialization;
[Collection(nameof(XmlConvertDefaultSettingsCollection))]
public class XmlConvertTest : Test
{
    public XmlConvertTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void DefaultSettings_ShouldBeNullByDefault()
    {
        var original = XmlConvert.DefaultSettings;
        try
        {
            XmlConvert.DefaultSettings = null;
            Assert.Null(XmlConvert.DefaultSettings);
        }
        finally
        {
            XmlConvert.DefaultSettings = original;
        }
    }

    [Fact]
    public void DefaultSettings_ShouldReturnConfiguredOptionsWhenSet()
    {
        var original = XmlConvert.DefaultSettings;
        try
        {
            var expected = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Custom") };
            XmlConvert.DefaultSettings = () => expected;

            var result = XmlConvert.DefaultSettings?.Invoke();

            Assert.NotNull(result);
            Assert.Equal("Custom", result.RootName.LocalName);
        }
        finally
        {
            XmlConvert.DefaultSettings = original;
        }
    }

    [Fact]
    public void DefaultSettings_ShouldBeUsedByXmlSerializerCreate_WhenSettingsArgumentIsNull()
    {
        var original = XmlConvert.DefaultSettings;
        try
        {
            var expected = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("FromDefault") };
            XmlConvert.DefaultSettings = () => expected;

            var serializer = XmlSerializer.Create(null);
            var result = serializer.Serialize("hello", typeof(string));
            var xml = result.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.NotNull(serializer);
            Assert.Contains("<FromDefault>", xml);
        }
        finally
        {
            XmlConvert.DefaultSettings = original;
        }
    }
}
