using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.Extensions.Xml
{
    public class XmlExtensionsTest : Test
    {
        public XmlExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void XmlExtensions_ShouldCreateReadersFromBytesStreamsAndUris()
        {
            var xml = "<?xml version=\"1.0\"?><root><!--comment--><item>42</item></root>";
            var bytes = Encoding.UTF8.GetBytes(xml);
            var filePath = Path.Combine(Environment.CurrentDirectory, $"xml-{Guid.NewGuid():N}.xml");
            File.WriteAllText(filePath, xml, Encoding.UTF8);
            try
            {
                using var byteReader = bytes.ToXmlReader(o => o.IgnoreComments = true);
                using var streamReader = new MemoryStream(bytes).ToXmlReader();
                using var uriReader = new Uri(filePath).ToXmlReader();

                Assert.True(byteReader.MoveToFirstElement());
                Assert.True(streamReader.MoveToFirstElement());
                Assert.True(uriReader.MoveToFirstElement());
                Assert.Equal("root", byteReader.LocalName);
                Assert.Equal("root", streamReader.LocalName);
                Assert.Equal("root", uriReader.LocalName);
                Assert.Throws<ArgumentNullException>(() => ByteArrayExtensions.ToXmlReader(null));
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ToXmlReader(null));
                Assert.Throws<ArgumentNullException>(() => UriExtensions.ToXmlReader(null));
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
