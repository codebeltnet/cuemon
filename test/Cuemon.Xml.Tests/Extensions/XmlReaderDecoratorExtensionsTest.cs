using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml;
public class XmlReaderDecoratorExtensionsTest : Test
{
    public XmlReaderDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    private static XmlReader CreateReaderFromXml(string xml)
    {
        var settings = new XmlReaderSettings { IgnoreWhitespace = true };
        return XmlReader.Create(new StringReader(xml), settings);
    }

    [Fact]
    public void MoveToFirstElement_ShouldReturnTrue_WhenElementExists()
    {
        using (var reader = CreateReaderFromXml("<root><child/></root>"))
        {
            var result = Decorator.Enclose(reader).MoveToFirstElement();
            Assert.True(result);
            Assert.Equal("root", reader.LocalName);
            TestOutput.WriteLine(reader.LocalName);
        }
    }

    [Fact]
    public void MoveToFirstElement_ShouldReturnFalse_WhenNoElements()
    {
        var settings = new XmlReaderSettings { IgnoreWhitespace = true, ConformanceLevel = System.Xml.ConformanceLevel.Fragment };
        using (var reader = XmlReader.Create(new StringReader("<!-- comment only -->"), settings))
        {
            var result = Decorator.Enclose(reader).MoveToFirstElement();
            Assert.False(result);
        }
    }

    [Fact]
    public void MoveToFirstElement_ShouldThrowArgumentException_WhenReaderAlreadyRead()
    {
        using (var reader = CreateReaderFromXml("<root/>"))
        {
            reader.Read();
            Assert.Throws<ArgumentException>(() => Decorator.Enclose(reader).MoveToFirstElement());
        }
    }

    [Fact]
    public void MoveToFirstElement_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XmlReaderDecoratorExtensions.MoveToFirstElement(null));
    }

    [Fact]
    public void Chunk_ShouldSplitXmlIntoChunks_WhenSizeIsOne()
    {
        const string xml = "<items><item id=\"1\"/><item id=\"2\"/><item id=\"3\"/></items>";
        using (var reader = CreateReaderFromXml(xml))
        {
            var chunks = new List<XmlReader>(Decorator.Enclose(reader).Chunk(size: 1));
            Assert.Equal(3, chunks.Count);
            TestOutput.WriteLine($"Chunk count: {chunks.Count}");
        }
    }

    [Fact]
    public void Chunk_ShouldReturnSingleChunk_WhenAllFitInSize()
    {
        const string xml = "<items><item id=\"1\"/><item id=\"2\"/></items>";
        using (var reader = CreateReaderFromXml(xml))
        {
            var chunks = new List<XmlReader>(Decorator.Enclose(reader).Chunk(size: 128));
            Assert.Equal(1, chunks.Count);
        }
    }

    [Fact]
    public void Chunk_ShouldThrowArgumentNullException_WhenDecoratorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new List<XmlReader>(XmlReaderDecoratorExtensions.Chunk(null));
        });
    }

    [Fact]
    public void Chunk_ShouldThrowArgumentException_WhenReaderAlreadyRead()
    {
        using (var reader = CreateReaderFromXml("<root><child/></root>"))
        {
            reader.Read();
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new List<XmlReader>(Decorator.Enclose(reader).Chunk());
            });
        }
    }

    [Fact]
    public void ToHierarchy_ShouldConvertXmlToHierarchy()
    {
        const string xml = "<root><name>Alice</name><age>30</age></root>";
        using (var reader = XmlReader.Create(new StringReader(xml)))
        {
            var hierarchy = Decorator.Enclose(reader).ToHierarchy();
            Assert.NotNull(hierarchy);
            TestOutput.WriteLine(hierarchy.ToString());
        }
    }

    [Fact]
    public void ToHierarchy_ShouldHandleAttributes()
    {
        const string xml = "<root id=\"1\" name=\"test\"><child/></root>";
        using (var reader = XmlReader.Create(new StringReader(xml)))
        {
            var hierarchy = Decorator.Enclose(reader).ToHierarchy();
            Assert.NotNull(hierarchy);
        }
    }

    [Fact]
    public void ToHierarchy_Null_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XmlReaderDecoratorExtensions.ToHierarchy(null));
    }
}
