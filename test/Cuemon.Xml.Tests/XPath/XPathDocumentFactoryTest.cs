using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.XPath;
public class XPathDocumentFactoryTest : Test
{
    public XPathDocumentFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    private const string SampleXml = "<root><child>value</child></root>";

    [Fact]
    public void CreateDocument_FromString_ShouldReturnXPathDocument()
    {
        var doc = XPathDocumentFactory.CreateDocument(SampleXml);
        var nav = doc.CreateNavigator();
        Assert.True(nav.MoveToChild("root", ""));
        TestOutput.WriteLine(nav.OuterXml);
    }

    [Fact]
    public void CreateDocument_FromString_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument((string)null));
    }

    [Fact]
    public void CreateDocument_FromStringAndEncoding_ShouldReturnXPathDocument()
    {
        var doc = XPathDocumentFactory.CreateDocument(SampleXml, Encoding.UTF8);
        var nav = doc.CreateNavigator();
        Assert.True(nav.MoveToChild("root", ""));
    }

    [Fact]
    public void CreateDocument_FromStringAndEncoding_NullString_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument(null, Encoding.UTF8));
    }

    [Fact]
    public void CreateDocument_FromStringAndEncoding_NullEncoding_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument(SampleXml, null));
    }

    [Fact]
    public void CreateDocument_FromStream_ShouldReturnXPathDocument()
    {
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(SampleXml)))
        {
            var doc = XPathDocumentFactory.CreateDocument(ms);
            var nav = doc.CreateNavigator();
            Assert.True(nav.MoveToChild("root", ""));
        }
    }

    [Fact]
    public void CreateDocument_FromStream_WithLeaveOpen_ShouldReturnXPathDocument()
    {
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(SampleXml));
        var doc = XPathDocumentFactory.CreateDocument(ms, leaveOpen: true);
        var nav = doc.CreateNavigator();
        Assert.True(nav.MoveToChild("root", ""));
        Assert.Equal(0, ms.Position);
        ms.Dispose();
    }

    [Fact]
    public void CreateDocument_FromStream_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument((Stream)null));
    }

    [Fact]
    public void CreateDocument_FromXmlReader_ShouldReturnXPathDocument()
    {
        using (var reader = XmlReader.Create(new StringReader(SampleXml)))
        {
            var doc = XPathDocumentFactory.CreateDocument(reader);
            var nav = doc.CreateNavigator();
            Assert.True(nav.MoveToChild("root", ""));
        }
    }

    [Fact]
    public void CreateDocument_FromXmlReader_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument((XmlReader)null));
    }

    [Fact]
    public void CreateDocument_FromUri_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XPathDocumentFactory.CreateDocument((Uri)null));
    }
}
