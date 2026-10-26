using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.IO;
public class TextReaderExtensionsTest : Test
{
    public TextReaderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task CopyToAsync_ShouldCopyContentOfReaderToWriter()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToTextReader();
        var sut3 = new MemoryStream();
        var sut4 = new StreamWriter(sut3);
        await sut2.CopyToAsync(sut4);
        await sut4.FlushAsync();
        var sut5 = await sut3.ToEncodedStringAsync();

        sut2.Dispose();

#if NET10_0_OR_GREATER
        await sut3.DisposeAsync();
        await sut4.DisposeAsync();
#else
        sut3.Dispose();
        sut4.Dispose();
#endif

        Assert.Equal(sut1, sut5);
    }

    [Fact]
    public void ReadAllLines_ShouldReadEverythingAsEnumerable()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToTextReader();
        var sut3 = sut2.ReadAllLines().ToList();

        Assert.True(sut3.Count == 1, "sut3.Count() == 1");
        Assert.Equal(sut1, sut3.Single());
    }

    [Fact]
    public async Task ReadAllLinesAsync_ShouldReadEverythingAsEnumerableAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToTextReader();
        var sut3 = await sut2.ReadAllLinesAsync();

        Assert.True(sut3.Count == 1, "sut3.Count() == 1");
        Assert.Equal(sut1, sut3.Single());
    }
}
