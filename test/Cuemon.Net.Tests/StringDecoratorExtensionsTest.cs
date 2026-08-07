using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net;
public class StringDecoratorExtensionsTest : Test
{
    public StringDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UrlEncode_ShouldEncodeAndDecodeStringToBeUrlCompliant()
    {
        Assert.Equal("a", Decorator.Enclose("a").UrlEncode());
        Assert.Equal("b", Decorator.Enclose("b").UrlEncode());
        Assert.Equal("c", Decorator.Enclose("c").UrlEncode());
        Assert.Equal("d", Decorator.Enclose("d").UrlEncode());
        Assert.Equal("%00", Decorator.Enclose("\0").UrlEncode());
        Assert.Equal("\0", Decorator.Enclose("%00").UrlDecode());
        Assert.Equal("%26", Decorator.Enclose("&").UrlEncode());
        Assert.Equal("&", Decorator.Enclose("%26").UrlDecode());
        Assert.Equal("%ef%bf%bf", Decorator.Enclose("\uFFFF").UrlEncode());
        Assert.Equal("\uFFFF", Decorator.Enclose("%ef%bf%bf").UrlDecode());
    }

    [Fact]
    public void UrlEncode_WithCharsRequiringEncodingAtBeginning()
    {
        Assert.Equal(@"%26Hello%2cthere!", Decorator.Enclose("&Hello,there!").UrlEncode());
    }

    [Fact]
    public void UrlEncode_WithCharsRequiringEncodingAtEnd()
    {
        Assert.Equal(@"Hello%2cthere!%26", Decorator.Enclose("Hello,there!&").UrlEncode());
    }

    [Fact]
    public void UrlEncode_WithCharsRequiringEncodingInMiddle()
    {
        Assert.Equal(@"Hello%2c+%26there!", Decorator.Enclose("Hello, &there!").UrlEncode());
    }

    [Fact]
    public void UrlEncode_WithCharsRequiringEncodingInterspersed()
    {
        Assert.Equal(@"Hello%2c+%3cthere%3e!", Decorator.Enclose("Hello, <there>!").UrlEncode());
    }

}
