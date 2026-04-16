using System;
using System.Xml.Serialization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Serialization
{
    public class XmlQualifiedEntityTest : Test
    {
        public XmlQualifiedEntityTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_WithLocalName_ShouldSetLocalName()
        {
            var sut = new XmlQualifiedEntity("Root");

            Assert.Equal("Root", sut.LocalName);
            Assert.Null(sut.Namespace);
            Assert.Null(sut.Prefix);
            Assert.False(sut.HasXmlAttributeDecoration);
            Assert.False(sut.HasXmlElementDecoration);
            Assert.False(sut.HasXmlAnyElementDecoration);
            Assert.False(sut.HasXmlRootDecoration);
        }

        [Fact]
        public void Ctor_WithLocalNameAndNamespace_ShouldSetBoth()
        {
            var sut = new XmlQualifiedEntity("Root", "https://example.com");

            Assert.Equal("Root", sut.LocalName);
            Assert.Equal("https://example.com", sut.Namespace);
            Assert.Null(sut.Prefix);
        }

        [Fact]
        public void Ctor_WithPrefixLocalNameAndNamespace_ShouldSetAll()
        {
            var sut = new XmlQualifiedEntity("ex", "Root", "https://example.com");

            Assert.Equal("ex", sut.Prefix);
            Assert.Equal("Root", sut.LocalName);
            Assert.Equal("https://example.com", sut.Namespace);
        }

        [Fact]
        public void Ctor_WithXmlElementAttribute_ShouldHaveXmlElementDecoration()
        {
            var attr = new XmlElementAttribute("ElementName", typeof(object)) { Namespace = "https://ns.example.com" };
            var sut = new XmlQualifiedEntity(attr);

            Assert.Equal("ElementName", sut.LocalName);
            Assert.Equal("https://ns.example.com", sut.Namespace);
            Assert.True(sut.HasXmlElementDecoration);
            Assert.False(sut.HasXmlAttributeDecoration);
            Assert.False(sut.HasXmlRootDecoration);
            Assert.False(sut.HasXmlAnyElementDecoration);
        }

        [Fact]
        public void Ctor_WithXmlElementAttribute_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlQualifiedEntity((XmlElementAttribute)null));
        }

        [Fact]
        public void Ctor_WithXmlAttributeAttribute_ShouldHaveXmlAttributeDecoration()
        {
            var attr = new XmlAttributeAttribute("AttrName") { Namespace = "https://attr.example.com" };
            var sut = new XmlQualifiedEntity(attr);

            Assert.Equal("AttrName", sut.LocalName);
            Assert.Equal("https://attr.example.com", sut.Namespace);
            Assert.True(sut.HasXmlAttributeDecoration);
            Assert.False(sut.HasXmlElementDecoration);
            Assert.False(sut.HasXmlRootDecoration);
            Assert.False(sut.HasXmlAnyElementDecoration);
        }

        [Fact]
        public void Ctor_WithXmlAttributeAttribute_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlQualifiedEntity((XmlAttributeAttribute)null));
        }

        [Fact]
        public void Ctor_WithXmlRootAttribute_ShouldHaveXmlRootDecoration()
        {
            var attr = new XmlRootAttribute("RootName") { Namespace = "https://root.example.com" };
            var sut = new XmlQualifiedEntity(attr);

            Assert.Equal("RootName", sut.LocalName);
            Assert.Equal("https://root.example.com", sut.Namespace);
            Assert.True(sut.HasXmlRootDecoration);
            Assert.False(sut.HasXmlAttributeDecoration);
            Assert.False(sut.HasXmlElementDecoration);
            Assert.False(sut.HasXmlAnyElementDecoration);
        }

        [Fact]
        public void Ctor_WithXmlRootAttribute_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlQualifiedEntity((XmlRootAttribute)null));
        }

        [Fact]
        public void Ctor_WithXmlAnyElementAttribute_ShouldHaveXmlAnyElementDecoration()
        {
            var attr = new XmlAnyElementAttribute("AnyElement") { Namespace = "https://any.example.com" };
            var sut = new XmlQualifiedEntity(attr);

            Assert.Equal("AnyElement", sut.LocalName);
            Assert.Equal("https://any.example.com", sut.Namespace);
            Assert.True(sut.HasXmlAnyElementDecoration);
            Assert.False(sut.HasXmlElementDecoration);
            Assert.False(sut.HasXmlAttributeDecoration);
            Assert.False(sut.HasXmlRootDecoration);
        }

        [Fact]
        public void Ctor_WithXmlAnyElementAttribute_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlQualifiedEntity((XmlAnyElementAttribute)null));
        }
    }
}
