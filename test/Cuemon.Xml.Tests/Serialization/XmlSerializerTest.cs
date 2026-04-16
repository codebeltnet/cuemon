using System;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Cuemon.Xml.Serialization.Converters;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    public class XmlSerializerTest : Test
    {
        public XmlSerializerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_WithNullSettings_AndNoDefaultSettings_ShouldProduceValidOutput()
        {
            var original = XmlConvert.DefaultSettings;
            try
            {
                XmlConvert.DefaultSettings = null;

                var sut = XmlSerializer.Create(null);
                var result = sut.Serialize("test", typeof(string));
                var xml = result.ToEncodedString();

                TestOutput.WriteLine(xml);
                Assert.NotNull(sut);
                Assert.Contains("test", xml);
            }
            finally
            {
                XmlConvert.DefaultSettings = original;
            }
        }

        [Fact]
        public void Create_WithExplicitSettings_ShouldApplySettings()
        {
            var options = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Explicit") };
            var sut = XmlSerializer.Create(options);

            var result = sut.Serialize("test", typeof(string));
            var xml = result.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.Contains("<Explicit>", xml);
        }

        [Fact]
        public void Create_WithNullSettings_ShouldFallbackToDefaultSettings()
        {
            var original = XmlConvert.DefaultSettings;
            try
            {
                var fromDefault = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Default") };
                XmlConvert.DefaultSettings = () => fromDefault;

                var sut = XmlSerializer.Create(null);
                var result = sut.Serialize("test", typeof(string));
                var xml = result.ToEncodedString();

                TestOutput.WriteLine(xml);
                Assert.Contains("<Default>", xml);
            }
            finally
            {
                XmlConvert.DefaultSettings = original;
            }
        }

        [Fact]
        public void Serialize_ShouldProduceValidXmlStream_ForString()
        {
            var sut = XmlSerializer.Create(null);

            var result = sut.Serialize("Hello", typeof(string));

            Assert.NotNull(result);
            var xml = result.ToEncodedString();
            TestOutput.WriteLine(xml);
            Assert.Contains("Hello", xml);
        }

        [Fact]
        public void Serialize_ShouldProduceValidXmlStream_ForInt()
        {
            var sut = XmlSerializer.Create(null);
            var result = sut.Serialize(42, typeof(int));

            Assert.NotNull(result);
            var xml = result.ToEncodedString();
            TestOutput.WriteLine(xml);
            Assert.Contains("42", xml);
        }

        [Fact]
        public void Serialize_WithCustomRootName_ShouldUseRootName()
        {
            var options = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Custom") };
            var sut = XmlSerializer.Create(options);

            var result = sut.Serialize("test", typeof(string));

            Assert.NotNull(result);
            var xml = result.ToEncodedString();
            TestOutput.WriteLine(xml);
            Assert.Contains("<Custom>", xml);
        }

        [Fact]
        public void Deserialize_Generic_ShouldReturnTypedObject()
        {
            var sut = XmlSerializer.Create(null);
            var stream = sut.Serialize("World", typeof(string));
            stream.Position = 0;

            var result = sut.Deserialize<string>(stream);

            Assert.Equal("World", result);
        }

        [Fact]
        public void Deserialize_ShouldReturnPrimitive_Int()
        {
            var sut = XmlSerializer.Create(null);
            var stream = sut.Serialize(99, typeof(int));
            stream.Position = 0;

            var result = (int)sut.Deserialize(stream, typeof(int));

            Assert.Equal(99, result);
        }

        [Fact]
        public void Serialize_WithDynamicConverter_RootNameFromSerializer_ShouldPropagateToOutput()
        {
            // Verifies that a DynamicXmlConverterCore with no RootName gets the serializer's RootName.
            var rootName = new XmlQualifiedEntity("Propagated");
            var options = new XmlSerializerOptions { RootName = rootName };

            var dynamicConverter = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) =>
                {
                    var elementName = q?.LocalName ?? "String";
                    w.WriteStartElement(elementName);
                    w.WriteString(v);
                    w.WriteEndElement();
                },
                rootEntity: null
            );
            options.Converters.Add(dynamicConverter);

            var sut = XmlSerializer.Create(options);
            var result = sut.Serialize("value", typeof(string));
            var xml = result.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.Contains("<Propagated>value</Propagated>", xml);
        }

        [Fact]
        public void Serialize_WhenNoConverterMatches_ShouldUseDefaultXmlConverter()
        {
            var options = new XmlSerializerOptions();
            var sut = XmlSerializer.Create(options);

            var result = sut.Serialize(Guid.Empty, typeof(Guid));
            var xml = result.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.Contains("<Guid>", xml);
        }

        [Fact]
        public void Serialize_UsesWriterSettingsFromOptions()
        {
            var options = new XmlSerializerOptions();
            options.Writer.OmitXmlDeclaration = true;
            var sut = XmlSerializer.Create(options);

            var result = sut.Serialize("test", typeof(string));
            var xml = result.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.DoesNotContain("<?xml", xml);
        }
    }
}
