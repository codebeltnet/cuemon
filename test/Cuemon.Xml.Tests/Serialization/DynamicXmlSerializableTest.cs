using System;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    public class DynamicXmlSerializableTest : Test
    {
        public DynamicXmlSerializableTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_WithNullSource_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DynamicXmlSerializable.Create<string>(null, (w, v) => { }));
        }

        [Fact]
        public void Create_WithValidSource_ShouldReturnIXmlSerializable()
        {
            var sut = DynamicXmlSerializable.Create("Hello", (w, v) => w.WriteString(v));

            Assert.NotNull(sut);
        }

        [Fact]
        public void WriteXml_ShouldInvokeWriterDelegate()
        {
            var written = string.Empty;
            var sut = DynamicXmlSerializable.Create("TestValue", (w, v) => { written = v; });

            using var ms = new System.IO.MemoryStream();
            using var writer = XmlWriter.Create(ms);
            sut.WriteXml(writer);

            Assert.Equal("TestValue", written);
        }

        [Fact]
        public void WriteXml_WithNullWriterDelegate_ShouldThrowNotImplementedException()
        {
            var sut = DynamicXmlSerializable.Create<string>("Source", null);

            using var ms = new System.IO.MemoryStream();
            using var writer = XmlWriter.Create(ms);

            Assert.Throws<NotImplementedException>(() => sut.WriteXml(writer));
        }

        [Fact]
        public void ReadXml_WithReaderDelegate_ShouldInvokeDelegate()
        {
            var readInvoked = false;
            var sut = DynamicXmlSerializable.Create("Source", (w, v) => { }, reader: r => { readInvoked = true; });

            using var reader = XmlReader.Create(new System.IO.StringReader("<x/>"));
            sut.ReadXml(reader);

            Assert.True(readInvoked);
        }

        [Fact]
        public void ReadXml_WithNullReaderDelegate_ShouldThrowNotImplementedException()
        {
            var sut = DynamicXmlSerializable.Create("Source", (w, v) => { });

            using var reader = XmlReader.Create(new System.IO.StringReader("<x/>"));

            Assert.Throws<NotImplementedException>(() => sut.ReadXml(reader));
        }

        [Fact]
        public void GetSchema_WithSchemaDelegate_ShouldReturnSchema()
        {
            var schema = new System.Xml.Schema.XmlSchema();
            var sut = DynamicXmlSerializable.Create("Source", (w, v) => { }, schema: () => schema);

            var result = sut.GetSchema();

            Assert.Same(schema, result);
        }

        [Fact]
        public void GetSchema_WithNullSchemaDelegate_ShouldThrowNotImplementedException()
        {
            var sut = DynamicXmlSerializable.Create("Source", (w, v) => { });

            Assert.Throws<NotImplementedException>(() => sut.GetSchema());
        }
    }
}
