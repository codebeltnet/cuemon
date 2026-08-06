using System;
using System.IO;
using System.Text;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml;
public class StreamDecoratorExtensionsTest : Test
{
    public StreamDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToXmlReader_ShouldReturnXmlReader_WhenValidXmlStream()
    {
        var xml = "<root><child>hello</child></root>";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            var reader = Decorator.Enclose(ms).ToXmlReader();
            Assert.NotNull(reader);
            reader.MoveToContent();
            Assert.Equal("root", reader.LocalName);
            TestOutput.WriteLine(reader.LocalName);
        }
    }

    [Fact]
    public void ToXmlReader_WithExplicitEncoding_ShouldReturnXmlReader()
    {
        var xml = "<root><child>hello</child></root>";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            var reader = Decorator.Enclose(ms).ToXmlReader(Encoding.UTF8);
            Assert.NotNull(reader);
            reader.MoveToContent();
            Assert.Equal("root", reader.LocalName);
        }
    }

    [Fact]
    public void ToXmlReader_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StreamDecoratorExtensions.ToXmlReader(null));
    }

    [Fact]
    public void TryDetectXmlEncoding_ShouldReturnTrueWithUtf8_WhenXmlDeclarationPresent()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            var result = Decorator.Enclose(ms).TryDetectXmlEncoding(out var encoding);
            Assert.True(result);
            Assert.NotNull(encoding);
            TestOutput.WriteLine(encoding.EncodingName);
        }
    }

    [Fact]
    public void TryDetectXmlEncoding_ShouldReturnFalse_WhenNoEncodingInfo()
    {
        var xml = "<root/>";
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            var result = Decorator.Enclose(ms).TryDetectXmlEncoding(out var encoding);
            Assert.False(result);
            Assert.NotNull(encoding);
        }
    }

    [Fact]
    public void TryDetectXmlEncoding_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StreamDecoratorExtensions.TryDetectXmlEncoding(null, out _));
    }
}
