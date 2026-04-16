using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization.Converters;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    public class XmlSerializerOptionsTest : Test
    {
        public XmlSerializerOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldHaveExpectedDefaults()
        {
            var sut = new XmlSerializerOptions();

            Assert.NotNull(sut.Writer);
            Assert.NotNull(sut.Reader);
            Assert.NotNull(sut.Converters);
            Assert.Empty(sut.Converters);
            Assert.Null(sut.RootName);
            Assert.False(sut.FlattenCollectionItems);
            Assert.Equal(Alphanumeric.Tab, sut.Writer.IndentChars);
            Assert.Equal(DtdProcessing.Ignore, sut.Reader.DtdProcessing);
        }

        [Fact]
        public void Writer_ShouldBeAssignable()
        {
            var sut = new XmlSerializerOptions();
            var newSettings = new XmlWriterSettings { Indent = true };
            sut.Writer = newSettings;

            Assert.Same(newSettings, sut.Writer);
            Assert.True(sut.Writer.Indent);
        }

        [Fact]
        public void Reader_ShouldBeAssignable()
        {
            var sut = new XmlSerializerOptions();
            var newSettings = new XmlReaderSettings { IgnoreComments = true };
            sut.Reader = newSettings;

            Assert.Same(newSettings, sut.Reader);
            Assert.True(sut.Reader.IgnoreComments);
        }

        [Fact]
        public void RootName_ShouldBeAssignable()
        {
            var sut = new XmlSerializerOptions();
            var rootName = new XmlQualifiedEntity("MyRoot");
            sut.RootName = rootName;

            Assert.Same(rootName, sut.RootName);
            Assert.Equal("MyRoot", sut.RootName.LocalName);
        }

        [Fact]
        public void Converters_ShouldAllowAddingConverters()
        {
            var sut = new XmlSerializerOptions();
            var converter = new ExceptionConverter();
            sut.Converters.Add(converter);

            Assert.Single(sut.Converters);
            Assert.Same(converter, sut.Converters[0]);
        }

        [Fact]
        public void FlattenCollectionItems_ShouldBeSettable()
        {
            var sut = new XmlSerializerOptions();
            sut.FlattenCollectionItems = true;

            Assert.True(sut.FlattenCollectionItems);
        }
    }
}
