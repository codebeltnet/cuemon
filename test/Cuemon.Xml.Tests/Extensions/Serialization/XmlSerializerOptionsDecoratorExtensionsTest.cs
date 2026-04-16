using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    [Collection(nameof(XmlConvertDefaultSettingsCollection))]
    public class XmlSerializerOptionsDecoratorExtensionsTest : Test
    {
        public XmlSerializerOptionsDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ApplyToDefaultSettings_ShouldSetXmlConvertDefaultSettings()
        {
            var original = XmlConvert.DefaultSettings;
            try
            {
                var options = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Applied") };

                Decorator.Enclose(options).ApplyToDefaultSettings();

                Assert.NotNull(XmlConvert.DefaultSettings);
                var result = XmlConvert.DefaultSettings();
                Assert.Same(options, result);
                Assert.Equal("Applied", result.RootName.LocalName);
            }
            finally
            {
                XmlConvert.DefaultSettings = original;
            }
        }

        [Fact]
        public void ApplyToDefaultSettings_WithNullDecorator_ShouldThrowArgumentNullException()
        {
            Assert.Throws<System.ArgumentNullException>(() =>
                XmlSerializerOptionsDecoratorExtensions.ApplyToDefaultSettings(null));
        }

        [Fact]
        public void ApplyToDefaultSettings_CalledTwice_ShouldOverwritePreviousSettings()
        {
            var original = XmlConvert.DefaultSettings;
            try
            {
                var options1 = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("First") };
                var options2 = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Second") };

                Decorator.Enclose(options1).ApplyToDefaultSettings();
                Decorator.Enclose(options2).ApplyToDefaultSettings();

                var result = XmlConvert.DefaultSettings();
                Assert.Equal("Second", result.RootName.LocalName);
            }
            finally
            {
                XmlConvert.DefaultSettings = original;
            }
        }
    }
}
