using System;
using System.IO;
using System.Text;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml;
public class XmlDocumentFactoryTest : Test
{
    public XmlDocumentFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    private static MemoryStream XmlStream(string xml = "<root><child>value</child></root>")
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(xml));
    }

    [Fact]
    public void CreateDocument_FromStream_ShouldReturnXmlDocument()
    {
        using (var ms = XmlStream())
        {
            var doc = XmlDocumentFactory.CreateDocument(ms);
            Assert.Equal("root", doc.DocumentElement.LocalName);
            Assert.Equal("value", doc.DocumentElement.FirstChild.InnerText);
            TestOutput.WriteLine(doc.DocumentElement.OuterXml);
        }
    }

    [Fact]
    public void CreateDocument_FromStream_WithLeaveOpen_ShouldKeepStreamOpen()
    {
        var ms = XmlStream();
        var doc = XmlDocumentFactory.CreateDocument(ms, leaveOpen: true);
        Assert.Equal("root", doc.DocumentElement.LocalName);
        Assert.Equal(0, ms.Position);
        ms.Dispose();
    }

    [Fact]
    public void CreateDocument_FromStream_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XmlDocumentFactory.CreateDocument((Stream)null));
    }

    [Fact]
    public void CreateDocument_FromXmlReader_ShouldReturnXmlDocument()
    {
        using (var reader = XmlReader.Create(new StringReader("<root><child>value</child></root>")))
        {
            var doc = XmlDocumentFactory.CreateDocument(reader);
            Assert.Equal("root", doc.DocumentElement.LocalName);
        }
    }

    [Fact]
    public void CreateDocument_FromXmlReader_WithLeaveOpen_ShouldReturnXmlDocument()
    {
        var reader = XmlReader.Create(new StringReader("<root><child>value</child></root>"));
        var doc = XmlDocumentFactory.CreateDocument(reader, leaveOpen: true);
        Assert.Equal("root", doc.DocumentElement.LocalName);
        reader.Dispose();
    }

    [Fact]
    public void CreateDocument_FromUri_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XmlDocumentFactory.CreateDocument((Uri)null));
    }

    [Fact]
    public void CreateDocument_FromString_ShouldReturnXmlDocument()
    {
        var doc = XmlDocumentFactory.CreateDocument("<root><child>value</child></root>");
        Assert.Equal("root", doc.DocumentElement.LocalName);
        Assert.Equal("value", doc.DocumentElement.FirstChild.InnerText);
        TestOutput.WriteLine(doc.OuterXml);
    }

    [Fact]
    public void CreateDocument_FromString_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XmlDocumentFactory.CreateDocument((string)null));
    }

    [Fact]
    public void CreateDocument_FromString_Whitespace_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => XmlDocumentFactory.CreateDocument("   "));
    }

    [Fact]
    public void CreateDocument_FromString_InvalidXml_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => XmlDocumentFactory.CreateDocument("this-is-not-xml"));
    }
}
