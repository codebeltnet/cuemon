using System.Linq;
using Cuemon.Data;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data;
public class QueryFormatExtensionsTest : Test
{
    public QueryFormatExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedQueryFormat()
    {
        var sut1 = Enumerable.Range(1, 10).ToList();
        var sut2 = QueryFormat.Delimited.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("1", s),
            s => Assert.Equal("2", s),
            s => Assert.Equal("3", s),
            s => Assert.Equal("4", s),
            s => Assert.Equal("5", s),
            s => Assert.Equal("6", s),
            s => Assert.Equal("7", s),
            s => Assert.Equal("8", s),
            s => Assert.Equal("9", s),
            s => Assert.Equal("10", s));
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedSquareBracketQueryFormat()
    {
        var sut1 = Enumerable.Range(1, 10).ToList();
        var sut2 = QueryFormat.DelimitedSquareBracket.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("[1]", s),
            s => Assert.Equal("[2]", s),
            s => Assert.Equal("[3]", s),
            s => Assert.Equal("[4]", s),
            s => Assert.Equal("[5]", s),
            s => Assert.Equal("[6]", s),
            s => Assert.Equal("[7]", s),
            s => Assert.Equal("[8]", s),
            s => Assert.Equal("[9]", s),
            s => Assert.Equal("[10]", s));
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedStringQueryFormat()
    {
        var sut1 = Enumerable.Range(1, 10).ToList();
        var sut2 = QueryFormat.DelimitedString.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("'1'", s),
            s => Assert.Equal("'2'", s),
            s => Assert.Equal("'3'", s),
            s => Assert.Equal("'4'", s),
            s => Assert.Equal("'5'", s),
            s => Assert.Equal("'6'", s),
            s => Assert.Equal("'7'", s),
            s => Assert.Equal("'8'", s),
            s => Assert.Equal("'9'", s),
            s => Assert.Equal("'10'", s));
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedQueryFormat_Long()
    {
        long counter = int.MaxValue;
        var sut1 = Generate.RangeOf(10, i => counter + i).ToList();
        var sut2 = QueryFormat.Delimited.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("2147483647", s),
            s => Assert.Equal("2147483648", s),
            s => Assert.Equal("2147483649", s),
            s => Assert.Equal("2147483650", s),
            s => Assert.Equal("2147483651", s),
            s => Assert.Equal("2147483652", s),
            s => Assert.Equal("2147483653", s),
            s => Assert.Equal("2147483654", s),
            s => Assert.Equal("2147483655", s),
            s => Assert.Equal("2147483656", s));
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedSquareBracketQueryFormat_Long()
    {
        long counter = int.MaxValue;
        var sut1 = Generate.RangeOf(10, i => counter + i).ToList();
        var sut2 = QueryFormat.DelimitedSquareBracket.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("[2147483647]", s),
            s => Assert.Equal("[2147483648]", s),
            s => Assert.Equal("[2147483649]", s),
            s => Assert.Equal("[2147483650]", s),
            s => Assert.Equal("[2147483651]", s),
            s => Assert.Equal("[2147483652]", s),
            s => Assert.Equal("[2147483653]", s),
            s => Assert.Equal("[2147483654]", s),
            s => Assert.Equal("[2147483655]", s),
            s => Assert.Equal("[2147483656]", s));
    }

    [Fact]
    public void Embed_ShouldWrapSequenceInDelimitedStringQueryFormat_Long()
    {
        long counter = int.MaxValue;
        var sut1 = Generate.RangeOf(10, i => counter + i).ToList();
        var sut2 = QueryFormat.DelimitedString.Embed(sut1);
        var sut3 = sut2.Split(',');

        TestOutput.WriteLine(sut2);

        Assert.Collection(sut3,
            s => Assert.Equal("'2147483647'", s),
            s => Assert.Equal("'2147483648'", s),
            s => Assert.Equal("'2147483649'", s),
            s => Assert.Equal("'2147483650'", s),
            s => Assert.Equal("'2147483651'", s),
            s => Assert.Equal("'2147483652'", s),
            s => Assert.Equal("'2147483653'", s),
            s => Assert.Equal("'2147483654'", s),
            s => Assert.Equal("'2147483655'", s),
            s => Assert.Equal("'2147483656'", s));
    }
}
