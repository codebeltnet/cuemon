using System;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Serialization.Converters;
public class XmlConverterTest : Test
{
    public XmlConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanRead_ShouldReturnTrueByDefault()
    {
        var sut = new ExceptionConverter();
        Assert.True(sut.CanRead);
    }

    [Fact]
    public void CanWrite_ShouldReturnTrueByDefault()
    {
        var sut = new ExceptionConverter();
        Assert.True(sut.CanWrite);
    }

    [Fact]
    public void CanConvert_Generic_ShouldReturnTrueForAssignableType()
    {
        var sut = new ExceptionConverter();
        Assert.True(sut.CanConvert(typeof(Exception)));
        Assert.True(sut.CanConvert(typeof(InvalidOperationException)));
    }

    [Fact]
    public void CanConvert_Generic_ShouldReturnFalseForUnrelatedType()
    {
        var sut = new ExceptionConverter();
        Assert.False(sut.CanConvert(typeof(string)));
        Assert.False(sut.CanConvert(typeof(int)));
    }

    [Fact]
    public void WriteXml_ObjectOverload_ShouldDelegateToTypedOverload()
    {
        var sut = new ExceptionConverter();
        var exception = new InvalidOperationException("Test");

        using var ms = new System.IO.MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });

        sut.WriteXml(writer, (object)exception, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new System.IO.StreamReader(ms).ReadToEnd();

        Assert.Contains("InvalidOperationException", xml);
        Assert.Contains("Test", xml);
    }

    [Fact]
    public void ReadXml_ObjectOverload_ShouldDelegateToTypedOverload()
    {
        var originalXml = "<InvalidOperationException namespace=\"System\"><Message>Test</Message></InvalidOperationException>";
        using var reader = XmlReader.Create(new System.IO.StringReader(originalXml));
        var sut = new ExceptionConverter();

        var result = sut.ReadXml(reader, typeof(InvalidOperationException));

        Assert.IsType<InvalidOperationException>(result);
    }

    [Fact]
    public void FailureConverter_CanRead_ShouldBeFalse()
    {
        var sut = new FailureConverter();
        Assert.False(sut.CanRead);
    }

    [Fact]
    public void FailureConverter_CanWrite_ShouldBeTrue()
    {
        var sut = new FailureConverter();
        Assert.True(sut.CanWrite);
    }

    [Fact]
    public void FailureConverter_CanConvert_ShouldReturnTrueForFailure()
    {
        var sut = new FailureConverter();
        Assert.True(sut.CanConvert(typeof(Cuemon.Diagnostics.Failure)));
    }

    [Fact]
    public void FailureConverter_CanConvert_ShouldReturnFalseForUnrelatedType()
    {
        var sut = new FailureConverter();
        Assert.False(sut.CanConvert(typeof(string)));
    }
}
