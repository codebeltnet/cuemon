using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Xunit;

namespace Cuemon.Xml;
public class XmlStreamFactoryTest : Test
{
    [Fact]
    public void CreateStream_XmlShouldHaveUtf8Encoding()
    {
        var xml = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("xml");
            writer.WriteAttributeString("name", "tæææææst");
            writer.WriteEndElement();
        });

        ByteOrderMark.TryDetectEncoding(xml, out var unicodeEncoding);
        Decorator.Enclose(xml).TryDetectXmlEncoding(out var xmlEncoding);

        Assert.Equal(xmlEncoding, unicodeEncoding);
        Assert.Equal(Encoding.UTF8, xmlEncoding);
    }
}
