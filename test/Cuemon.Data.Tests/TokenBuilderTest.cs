using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data;
public class TokenBuilderTest : Test
{
    public TokenBuilderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ShouldTrackTokensAndQuotedDelimiters()
    {
        var sut = new TokenBuilder(',', '"', 3);

        sut.Append("a,\"b,c\",d,e");

        Assert.True(sut.IsValid);
        Assert.Equal(3, sut.Tokens);
        Assert.Equal(',', sut.Delimiter);
        Assert.Equal('"', sut.Qualifier);
        Assert.Equal("a,\"b,c\",d,", sut.ToString());
    }

    [Fact]
    public void ShouldHandleNullAndInvalidStringArguments()
    {
        var sut = new TokenBuilder(",", "\"", 2);

        sut.Append(null).Append("onlyone");

        Assert.False(sut.IsValid);
        Assert.Equal("onlyone", sut.ToString());
        Assert.Throws<System.FormatException>(() => new TokenBuilder("::", "\"", 1));
        Assert.Throws<System.FormatException>(() => new TokenBuilder(",", "''", 1));
    }
}
