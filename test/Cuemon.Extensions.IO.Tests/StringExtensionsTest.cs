using System;
using System.IO;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.IO;
public class StringExtensionsTest : Test
{
    public StringExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToStream_ShouldConvertStringToStream()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToStream();
        var sut3 = sut2.ToEncodedString();

        Assert.Equal(sut1, sut3);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
    }

    [Fact]
    public async Task ToStreamAsync_ShouldConvertStringToStreamAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = await sut1.ToStreamAsync();
        var sut3 = await sut2.ToEncodedStringAsync();

        Assert.Equal(sut1, sut3);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
    }

    [Fact]
    public void ToTextReader_ShouldConvertStringToTextReader()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        TextReader sut2 = null;
        string sut3 = null;
        using (sut2 = sut1.ToTextReader())
        {
            sut3 = sut2.ReadToEnd();
        }

        Assert.Equal(sut1, sut3);
        Assert.Throws<ObjectDisposedException>(() => sut2.Peek());
    }
}
