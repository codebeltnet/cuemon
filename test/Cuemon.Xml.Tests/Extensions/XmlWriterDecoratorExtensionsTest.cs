using System;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization;
using Xunit;

namespace Cuemon.Xml;
public class XmlWriterDecoratorExtensionsTest : Test
{
    public XmlWriterDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    private static (MemoryStream Stream, XmlWriter Writer) CreateWriter()
    {
        var ms = new MemoryStream();
        var settings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true };
        return (ms, XmlWriter.Create(ms, settings));
    }

    [Fact]
    public void WriteObject_Generic_ShouldSerializeObject()
    {
        var (ms, writer) = CreateWriter();
        using (writer)
        {
            Decorator.Enclose(writer).WriteObject("hello");
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("hello", xml);
        TestOutput.WriteLine(xml);
    }

    [Fact]
    public void WriteObject_WithType_ShouldSerializeObject()
    {
        var (ms, writer) = CreateWriter();
        using (writer)
        {
            Decorator.Enclose(writer).WriteObject("hello", typeof(string));
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("hello", xml);
    }

    [Fact]
    public void WriteObject_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlWriterDecoratorExtensions.WriteObject<string>(null, "value"));
    }

    [Fact]
    public void WriteObject_WithType_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlWriterDecoratorExtensions.WriteObject(null, "value", typeof(string)));
    }

    [Fact]
    public void WriteStartElement_ShouldWriteElement()
    {
        var (ms, writer) = CreateWriter();
        using (writer)
        {
            var entity = new XmlQualifiedEntity("myElement");
            Decorator.Enclose(writer).WriteStartElement(entity);
            writer.WriteEndElement();
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("myElement", xml);
        TestOutput.WriteLine(xml);
    }

    [Fact]
    public void WriteStartElement_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlWriterDecoratorExtensions.WriteStartElement(null, new XmlQualifiedEntity("test")));
    }

    [Fact]
    public void WriteEncapsulatingElementIfNotNull_WithElementName_ShouldWrapInElement()
    {
        var (ms, writer) = CreateWriter();
        using (writer)
        {
            var entity = new XmlQualifiedEntity("wrapper");
            Decorator.Enclose(writer).WriteEncapsulatingElementIfNotNull("content", entity, (w, v) => w.WriteString(v));
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("wrapper", xml);
        Assert.Contains("content", xml);
        TestOutput.WriteLine(xml);
    }

    [Fact]
    public void WriteEncapsulatingElementIfNotNull_WithNullElementName_ShouldNotWrap()
    {
        var ms = new MemoryStream();
        var settings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true, ConformanceLevel = System.Xml.ConformanceLevel.Fragment };
        using (var writer = XmlWriter.Create(ms, settings))
        {
            Decorator.Enclose(writer).WriteEncapsulatingElementIfNotNull("content", null, (w, v) => w.WriteString(v));
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("content", xml);
    }

    [Fact]
    public void WriteEncapsulatingElementIfNotNull_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlWriterDecoratorExtensions.WriteEncapsulatingElementIfNotNull<string>(
                null, "value", new XmlQualifiedEntity("x"), (w, v) => w.WriteString(v)));
    }

    [Fact]
    public void WriteXmlRootElement_ShouldWriteRootElement()
    {
        var (ms, writer) = CreateWriter();
        using (writer)
        {
            Decorator.Enclose(writer).WriteXmlRootElement("hello", (w, v, entity) => w.WriteString(v));
        }
        var xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        Assert.Contains("hello", xml);
        TestOutput.WriteLine(xml);
    }

    [Fact]
    public void WriteXmlRootElement_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlWriterDecoratorExtensions.WriteXmlRootElement<string>(
                null, "value", (w, v, entity) => w.WriteString(v)));
    }
}
