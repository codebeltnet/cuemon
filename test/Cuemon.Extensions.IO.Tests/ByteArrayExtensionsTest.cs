using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.IO;
public class ByteArrayExtensionsTest : Test
{
    public ByteArrayExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToStream_ShouldConvertByteArrayToStream()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = sut2.ToStream();
        var sut4 = sut3.ToByteArray();
        var sut5 = sut4.ToEncodedString();

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
        Assert.Throws<ObjectDisposedException>(() => sut3.Position);
    }

    [Fact]
    public async Task ToStreamAsync_ShouldConvertByteArrayToStreamAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = await sut2.ToStreamAsync();
        var sut4 = await sut3.ToByteArrayAsync();
        var sut5 = sut4.ToEncodedString();

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
        Assert.Throws<ObjectDisposedException>(() => sut3.Position);
    }
}
