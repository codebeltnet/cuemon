using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class CharExtensionsTest : Test
{
    public CharExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEnumerable_ShouldConvertCharSequenceToStringSequence()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToCharArray();
        var sut3 = sut2.ToEnumerable();
        var sut4 = string.Join("", sut3);
        var sut5 = sut2.FromChars();
        var sut6 = new string(sut2);

        Assert.Equal(sut1, sut4);
        Assert.Equal(sut1, sut5);
        Assert.Equal(sut5, sut6);
    }
}
