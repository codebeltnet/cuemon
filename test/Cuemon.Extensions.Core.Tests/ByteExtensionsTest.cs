using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Xunit;

namespace Cuemon.Extensions;
public class ByteExtensionsTest : Test
{
    public ByteExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToHexadecimalString_ShouldConvertByteArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = sut2.ToHexadecimalString();
        var sut4 = sut3.FromHexadecimal();
        var sut5 = sut4.ToByteArray();
        var sut6 = sut5.ToEncodedString();

        TestOutput.WriteLine(sut3);
        TestOutput.WriteLine(sut4);

        Assert.Equal(sut1, sut6);
        Assert.Equal(sut2, sut5);
        Assert.Equal(sut1, sut4);
    }

    [Fact]
    public void ToBinaryString_ShouldConvertByteArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = sut2.ToBinaryString();
        var sut4 = sut3.FromBinaryDigits();
        var sut5 = sut4.ToEncodedString();

        TestOutput.WriteLine(sut3);

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
    }

    [Fact]
    public void ToUrlEncodedBase64String_ShouldConvertByteArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = sut2.ToUrlEncodedBase64String();
        var sut4 = sut3.FromUrlEncodedBase64();
        var sut5 = sut4.ToEncodedString();

        TestOutput.WriteLine(sut3);

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
    }

    [Fact]
    public void ToBase64String_ShouldConvertByteArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = sut2.ToBase64String();
        var sut4 = sut3.FromBase64();
        var sut5 = sut4.ToEncodedString();

        TestOutput.WriteLine(sut3);

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
    }

    [Fact]
    public void TryDetectUnicodeEncoding_ShouldDetectUnicodeEncodings()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray(o => o.Preamble = PreambleSequence.Keep);
        sut2.TryDetectUnicodeEncoding(out var sut3);
        var sut4 = sut1.ToByteArray(o =>
        {
            o.Encoding = Encoding.Unicode;
            o.Preamble = PreambleSequence.Keep;
        });
        sut4.TryDetectUnicodeEncoding(out var sut5);
        var sut6 = sut1.ToByteArray(o =>
        {
            o.Encoding = Encoding.BigEndianUnicode;
            o.Preamble = PreambleSequence.Keep;
        });
        sut6.TryDetectUnicodeEncoding(out var sut7);
        var sut8 = sut1.ToByteArray(o =>
        {
            o.Encoding = Encoding.UTF32;
            o.Preamble = PreambleSequence.Keep;
        });
        sut8.TryDetectUnicodeEncoding(out var sut9);
        var sut10 = sut1.ToByteArray(o =>
        {
            o.Encoding = Encoding.GetEncoding("UTF-32BE");
            o.Preamble = PreambleSequence.Keep;
        });
        sut10.TryDetectUnicodeEncoding(out var sut11);

        Assert.Equal(Encoding.UTF8, sut3);
        Assert.Equal(Encoding.Unicode, sut5);
        Assert.Equal(Encoding.BigEndianUnicode, sut7);
        Assert.Equal(Encoding.UTF32, sut9);
        Assert.Equal(Encoding.GetEncoding("UTF-32BE"), sut11);
    }
}
