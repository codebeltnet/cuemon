using System.Xml.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Linq;
public class StringDecoratorExtensionsTest : Test
{
    public StringDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TryParseXElement_ShouldReturnTrue_WhenValidXml()
    {
        var result = Decorator.Enclose("<root><child>value</child></root>").TryParseXElement(out var element);
        Assert.True(result);
        Assert.NotNull(element);
        Assert.Equal("root", element.Name.LocalName);
        TestOutput.WriteLine(element.ToString());
    }

    [Fact]
    public void TryParseXElement_ShouldReturnFalse_WhenInvalidXml()
    {
        var result = Decorator.Enclose("<invalid>unclosed").TryParseXElement(out var element);
        Assert.False(result);
        Assert.Null(element);
    }

    [Fact]
    public void TryParseXElement_ShouldReturnFalse_WhenNotStartingWithAngleBracket()
    {
        var result = Decorator.Enclose("not-xml").TryParseXElement(out var element);
        Assert.False(result);
        Assert.Null(element);
    }

    [Fact]
    public void TryParseXElement_ShouldReturnFalse_WhenWhitespace()
    {
        var result = Decorator.Enclose("   ").TryParseXElement(out var element);
        Assert.False(result);
        Assert.Null(element);
    }

    [Fact]
    public void TryParseXElement_WithLoadOptions_ShouldReturnTrue_WhenValidXml()
    {
        var result = Decorator.Enclose("<root><child>value</child></root>").TryParseXElement(LoadOptions.None, out var element);
        Assert.True(result);
        Assert.NotNull(element);
    }

    [Fact]
    public void IsXmlString_ShouldReturnTrue_WhenValidXml()
    {
        var result = Decorator.Enclose("<root/>").IsXmlString();
        Assert.True(result);
    }

    [Fact]
    public void IsXmlString_ShouldReturnFalse_WhenNotXml()
    {
        var result = Decorator.Enclose("plain text").IsXmlString();
        Assert.False(result);
    }

    [Fact]
    public void IsXmlString_ShouldReturnFalse_WhenEmpty()
    {
        var result = Decorator.Enclose("").IsXmlString();
        Assert.False(result);
    }
}
