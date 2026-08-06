using AutoFixture;
using Xunit;

namespace Cuemon.Text;
public class StemTest
{
    [Fact]
    public void Stem_Value_Must_Start_With_Forward_Slash()
    {
        var fixture = new Fixture();
        var stem = fixture.Create<string>();
        var affix = "/";
        var result = new Stem(stem).AttachPrefix(affix);
        Assert.StartsWith(affix, result);
    }

    [Fact]
    public void Stem_Value_Must_End_With_Forward_Slash()
    {
        var fixture = new Fixture();
        var stem = fixture.Create<string>();
        var affix = "/";
        var result = new Stem(stem).AttachSuffix(affix);
        Assert.EndsWith(affix, result);
    }

    [Fact]
    public void Stem_Value_Must_Remain_Unaltered()
    {
        var fixture = new Fixture();
        var stem = fixture.Create<string>();
        var result = new Stem(stem).AttachSuffix(null).AttachPrefix("");
        Assert.Equal(stem, result);
    }

    [Fact]
    public void Stem_Value_Must_Start_With_Forward_Slash_And_End_With_Forward_Slash()
    {
        var fixture = new Fixture();
        var stem = fixture.Create<string>();
        var affix = "/";
        var result = new Stem(stem).AttachSuffix(affix).AttachPrefix(affix);
        Assert.StartsWith(affix, result);
        Assert.EndsWith(affix, result);
    }
}
