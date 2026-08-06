using System.Linq;
using System.Text;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Cuemon.Xml;
using Xunit;

namespace Cuemon.Extensions.Xml;
public class StreamExtensionsTest : Test
{
    private static readonly Encoding Iso88591 = Encoding.GetEncoding("ISO-8859-1");

    public StreamExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CopyXmlStream_ShouldChangeEncoding()
    {
        var utf8Xml = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("xml");
            writer.WriteAttributeString("name", "tæææææst");
            writer.WriteEndElement();
        });
        var utf8XmlBytesLength = utf8Xml.Length;

        Assert.True(utf8Xml.TryDetectUnicodeEncoding(out var unicodeEncoding));
        Assert.True(utf8Xml.TryDetectXmlEncoding(out var xmlEncoding));

        Assert.Equal(xmlEncoding, unicodeEncoding);
        Assert.Equal(Encoding.UTF8, xmlEncoding);

        TestOutput.WriteLine(utf8Xml.ToEncodedString(o => o.LeaveOpen = true));

        var utf16Xml = utf8Xml.CopyXmlStream(o => o.Encoding = Encoding.Unicode);
        var utf16XmlBytesLength = utf16Xml.Length;

        Assert.True(utf16XmlBytesLength > utf8XmlBytesLength, "utf16XmlBytesLength > utf8XmlBytesLength");
        Assert.True(utf16Xml.TryDetectUnicodeEncoding(out unicodeEncoding));
        Assert.True(utf16Xml.TryDetectXmlEncoding(out xmlEncoding));

        Assert.Equal(xmlEncoding, unicodeEncoding);
        Assert.Equal(Encoding.Unicode, xmlEncoding);

        TestOutput.WriteLine(utf16Xml.ToEncodedString(o => o.Encoding = Encoding.Unicode));

        var utf32Xml = utf8Xml.CopyXmlStream(o => o.Encoding = Encoding.UTF32);
        var utf32XmlBytesLength = utf32Xml.Length;

        Assert.True(utf32XmlBytesLength > utf16XmlBytesLength, "utf32XmlBytesLength > utf16XmlBytesLength");
        Assert.True(utf32Xml.TryDetectUnicodeEncoding(out unicodeEncoding));
        Assert.True(utf32Xml.TryDetectXmlEncoding(out xmlEncoding));

        Assert.Equal(xmlEncoding, unicodeEncoding);
        Assert.Equal(Encoding.UTF32, xmlEncoding);

        TestOutput.WriteLine(utf32Xml.ToEncodedString(o => o.Encoding = Encoding.UTF32));

        var iso88591Xml = utf8Xml.CopyXmlStream(o => o.Encoding = Iso88591);
        var iso88591XmlBytesLength = iso88591Xml.Length;

        Assert.True(utf16XmlBytesLength > iso88591XmlBytesLength); // BOM
        Assert.False(iso88591Xml.TryDetectUnicodeEncoding(out unicodeEncoding));
        Assert.True(iso88591Xml.TryDetectXmlEncoding(out xmlEncoding));

        Assert.Null(unicodeEncoding);
        Assert.Equal(Iso88591, xmlEncoding);

        TestOutput.WriteLine(iso88591Xml.ToEncodedString(o => o.Encoding = Iso88591));
    }

    [Fact]
    public void RemoveXmlNamespaceDeclarations_ShouldLoadAndClearAnyNamespaceDeclarations()
    {
        using (var file = typeof(StreamExtensionsTest).GetEmbeddedResources("Namespace.xml", ManifestResourceMatch.ContainsName).Values.Single())
        {
            var original = file.ToEncodedString(o => o.LeaveOpen = true);
            var sanitized = file.RemoveXmlNamespaceDeclarations().ToEncodedString();

            TestOutput.WriteLine(original);
            TestOutput.WriteLine("");
            TestOutput.WriteLine(sanitized);

            Assert.Contains("xmlns:h=\"http://www.w3.org/HTML/1998/html4\"", original);
            Assert.Contains("xmlns:xdc=\"http://www.xml.com/books\"", original);
            Assert.Contains("h:body", original);
            Assert.Contains("xdc:bookreview", original);
            Assert.True(original.Length > sanitized.Length, "original.Length > sanitized.Length");
            Assert.DoesNotContain("xmlns:h=\"http://www.w3.org/HTML/1998/html4\"", sanitized);
            Assert.DoesNotContain("xmlns:xdc=\"http://www.xml.com/books\"", sanitized);
            Assert.DoesNotContain("h:body", sanitized);
            Assert.DoesNotContain("xdc:bookreview", sanitized);
        }
    }
}
