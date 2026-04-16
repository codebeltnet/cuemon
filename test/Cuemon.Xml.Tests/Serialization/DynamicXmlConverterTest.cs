using System;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    public class DynamicXmlConverterTest : Test
    {
        public DynamicXmlConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_Generic_WithWriter_ShouldProduceWriteableConverter()
        {
            var sut = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) =>
                {
                    w.WriteStartElement("Custom");
                    w.WriteString(v);
                    w.WriteEndElement();
                }
            );

            Assert.True(sut.CanWrite);
            Assert.False(sut.CanRead);
        }

        [Fact]
        public void Create_Generic_WithReader_ShouldProduceReadableConverter()
        {
            var sut = DynamicXmlConverter.Create<string>(
                reader: (r, t) => "read-value"
            );

            Assert.False(sut.CanWrite);
            Assert.True(sut.CanRead);
        }

        [Fact]
        public void Create_Generic_WithBothDelegates_ShouldProduceBothCapabilities()
        {
            var sut = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) => { },
                reader: (r, t) => "value"
            );

            Assert.True(sut.CanWrite);
            Assert.True(sut.CanRead);
        }

        [Fact]
        public void Create_NonGeneric_WithObjectType_ShouldProduceConverter()
        {
            var sut = DynamicXmlConverter.Create(
                typeof(int),
                writer: (w, v, q) =>
                {
                    w.WriteStartElement("Int32");
                    w.WriteValue((int)v);
                    w.WriteEndElement();
                }
            );

            Assert.True(sut.CanWrite);
        }

        [Fact]
        public void CanConvert_ShouldReturnTrueForExactType()
        {
            var sut = DynamicXmlConverter.Create<string>(writer: (w, v, q) => { });
            Assert.True(sut.CanConvert(typeof(string)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalseForUnrelatedType()
        {
            var sut = DynamicXmlConverter.Create<string>(writer: (w, v, q) => { });
            Assert.False(sut.CanConvert(typeof(int)));
        }

        [Fact]
        public void CanConvert_WithPredicate_ShouldRespectPredicate()
        {
            var sut = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) => { },
                canConvertPredicate: t => t == typeof(string)
            );

            Assert.True(sut.CanConvert(typeof(string)));
            Assert.False(sut.CanConvert(typeof(object)));
        }

        [Fact]
        public void WriteXml_WithNullWriter_ShouldThrowInvalidOperationException()
        {
            var sut = DynamicXmlConverter.Create<string>();

            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms);

            Assert.Throws<InvalidOperationException>(() => sut.WriteXml(writer, "test", null));
        }

        [Fact]
        public void ReadXml_WithNullReader_ShouldThrowInvalidOperationException()
        {
            var sut = DynamicXmlConverter.Create<string>();

            using var reader = XmlReader.Create(new StringReader("<x>1</x>"));

            Assert.Throws<InvalidOperationException>(() => sut.ReadXml(reader, typeof(string)));
        }

        [Fact]
        public void WriteXml_ShouldInvokeWriterDelegate()
        {
            var sut = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) =>
                {
                    w.WriteStartElement("Result");
                    w.WriteString(v);
                    w.WriteEndElement();
                }
            );

            var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                sut.WriteXml(writer, "MyValue", null);
            }
            ms.Position = 0;
            var xml = ms.ToEncodedString();

            TestOutput.WriteLine(xml);
            Assert.Contains("<Result>MyValue</Result>", xml);
        }

        [Fact]
        public void ReadXml_ShouldInvokeReaderDelegate()
        {
            var sut = DynamicXmlConverter.Create<string>(
                reader: (r, t) => "from-reader"
            );

            using var reader = XmlReader.Create(new StringReader("<String>anything</String>"));
            var result = sut.ReadXml(reader, typeof(string));

            Assert.Equal("from-reader", result);
        }

        [Fact]
        public void RootName_WhenSetViaFactory_ShouldBeUsedInWriteXml()
        {
            var root = new XmlQualifiedEntity("MyRoot");
            var invoked = false;
            XmlQualifiedEntity capturedEntity = null;

            var sut = DynamicXmlConverter.Create<string>(
                writer: (w, v, q) =>
                {
                    invoked = true;
                    capturedEntity = q;
                },
                rootEntity: root
            );

            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms);
            sut.WriteXml(writer, "test", null);

            Assert.True(invoked);
            Assert.Equal("MyRoot", capturedEntity?.LocalName);
        }

        [Fact]
        public void DynamicXmlConverterCore_RootName_ShouldBeSettable()
        {
            var core = (DynamicXmlConverterCore)DynamicXmlConverter.Create<string>(
                writer: (w, v, q) => { }
            );

            core.RootName = new XmlQualifiedEntity("UpdatedRoot");

            Assert.Equal("UpdatedRoot", core.RootName.LocalName);
        }
    }
}
