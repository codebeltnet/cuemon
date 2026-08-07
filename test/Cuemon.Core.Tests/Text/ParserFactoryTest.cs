using System;
using System.ComponentModel;
using System.Globalization;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Xunit;

namespace Cuemon.Text;
public class ParserFactoryTest : Test
{
    public ParserFactoryTest(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void ParserFactory_ShouldConvertString_ToGuid()
    {
        var guid = Guid.NewGuid();

        var nguid = guid.ToString("N");
        var dguid = guid.ToString("D");
        var bguid = guid.ToString("B");
        var pguid = guid.ToString("P");
        var xguid = guid.ToString("X");

        TestOutput.WriteLine($"N: {nguid}");
        TestOutput.WriteLine($"D: {dguid}");
        TestOutput.WriteLine($"B: {bguid}");
        TestOutput.WriteLine($"P: {pguid}");
        TestOutput.WriteLine($"X: {xguid}");

        Assert.Equal(guid, ParserFactory.FromGuid().Parse(nguid, o => o.Formats = GuidFormats.N));
        Assert.Equal(guid, ParserFactory.FromGuid().Parse(dguid, o => o.Formats = GuidFormats.D));
        Assert.Equal(guid, ParserFactory.FromGuid().Parse(bguid, o => o.Formats = GuidFormats.B));
        Assert.Equal(guid, ParserFactory.FromGuid().Parse(pguid, o => o.Formats = GuidFormats.P));
        Assert.Equal(guid, ParserFactory.FromGuid().Parse(xguid, o => o.Formats = GuidFormats.X));

        Assert.False(ParserFactory.FromGuid().TryParse(xguid, out _));
        Assert.False(ParserFactory.FromGuid().TryParse(nguid, out _));
        Assert.True(ParserFactory.FromGuid().TryParse(pguid, out _));
        Assert.True(ParserFactory.FromGuid().TryParse(dguid, out _));
        Assert.True(ParserFactory.FromGuid().TryParse(bguid, out _));

        Assert.True(ParserFactory.FromGuid().TryParse(pguid, out _, o => o.Formats = GuidFormats.Any));
        Assert.True(ParserFactory.FromGuid().TryParse(dguid, out _, o => o.Formats = GuidFormats.Any));
        Assert.True(ParserFactory.FromGuid().TryParse(bguid, out _, o => o.Formats = GuidFormats.Any));
        Assert.True(ParserFactory.FromGuid().TryParse(nguid, out _, o => o.Formats = GuidFormats.Any));
        Assert.True(ParserFactory.FromGuid().TryParse(xguid, out _, o => o.Formats = GuidFormats.Any));
    }

    [Fact]
    public void ParserFactory_ShouldConvertUrlEncodedBase64String_ToByteArray()
    {
        var ts = "This is a test with special characters !\"#¤%&/()@!=";
        var tsByteArray = Decorator.Enclose(ts).ToByteArray();
        var tsExpectedResult = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ";

        var c1 = ParserFactory.FromUrlEncodedBase64().Parse(tsExpectedResult);
        Assert.Equal(c1, tsByteArray);

        ParserFactory.FromUrlEncodedBase64().TryParse(tsExpectedResult, out var c2);
        Assert.Equal(c2, tsByteArray);


        Assert.Throws<FormatException>(() =>
        {
            ParserFactory.FromUrlEncodedBase64().Parse("invalidbase64");
        });

        Assert.False(ParserFactory.FromUrlEncodedBase64().TryParse("invalidbase64", out var c3), "Should have failed given wrong base64 string.");
        Assert.Equal(default, c3);
    }

    [Fact]
    public void ParserFactory_ShouldConvertString_ToTypeConverterImplementation()
    {
        var uri = new Uri("https://www.cuemon.net/");
        Assert.Equal(uri, ParserFactory.FromObject().Parse<Uri>(uri.OriginalString));

        var sg = Guid.NewGuid();
        Assert.Equal(sg, ParserFactory.FromObject().Parse<Guid>(sg.ToString()));

#if NET9_0_OR_GREATER
        var v = new Version();
        Assert.Equal(v, ParserFactory.FromObject().Parse(v.ToString(), typeof(Version)));
#endif

        var ts = TimeSpan.FromMinutes(42);
        Assert.Equal(ts, ParserFactory.FromObject().Parse(ts.ToString(), typeof(TimeSpan)));
    }

    [Fact]
    public void ParserFactory_ShouldConvertProtocolRelativeUrlString_ToUri()
    {
        var o = "//www.cuemon.net/about";
        var x = ParserFactory.FromProtocolRelativeUri().Parse(o);
        var y = StringFactory.CreateProtocolRelativeUrl(x);

        TestOutput.WriteLine($"Input: {o}");
        TestOutput.WriteLine($"Conversion: {x}");
        TestOutput.WriteLine($"Reversed: {y}");

        Assert.Equal(o, y);
    }

    [Fact]
    public void ParserFactory_ShouldConvertBinaryDigitsString_ToByteArray()
    {
        var ts = "This is a test with special characters !\"#¤%&/()@!=";
        var tsByteArray = Decorator.Enclose(ts).ToByteArray();
        var tsExpectedResult = "01010100011010000110100101110011001000000110100101110011001000000110000100100000011101000110010101110011011101000010000001110111011010010111010001101000001000000111001101110000011001010110001101101001011000010110110000100000011000110110100001100001011100100110000101100011011101000110010101110010011100110010000000100001001000100010001111000010101001000010010100100110001011110010100000101001010000000010000100111101";

        var c1 = ParserFactory.FromBinaryDigits().Parse(tsExpectedResult);
        Assert.Equal(c1, tsByteArray);

        ParserFactory.FromBinaryDigits().TryParse(tsExpectedResult, out var c2);
        Assert.Equal(c2, tsByteArray);

        Assert.Throws<FormatException>(() =>
        {
            ParserFactory.FromBinaryDigits().Parse("invalidBinary");
        });

        Assert.False(ParserFactory.FromBinaryDigits().TryParse("invalidBinary", out var c3), "Should have failed given wrong binary string.");

        Assert.Equal(default, c3);
    }


    [Fact]
    public void ParserFactory_ShouldConvertBase64String_ToByteArray()
    {
        var ts = "This is a test with special characters !\"#¤%&/()@!=";
        var tsByteArray = Decorator.Enclose(ts).ToByteArray();
        var tsExpectedResult = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ==";

        var c1 = ParserFactory.FromBase64().Parse(tsExpectedResult);
        Assert.Equal(c1, tsByteArray);

        ParserFactory.FromBase64().TryParse(tsExpectedResult, out var c2);
        Assert.Equal(c2, tsByteArray);

        Assert.Throws<FormatException>(() =>
        {
            ParserFactory.FromBinaryDigits().Parse("invalidBase64");
        });

        Assert.False(ParserFactory.FromBinaryDigits().TryParse("invalidBase64", out var c3), "Should have failed given wrong base64 string.");

        Assert.Equal(default, c3);
    }

    [Fact]
    public void ParserFactory_ShouldConvertHexadecimalString_ToByteArray()
    {
        var ts = "This is a text that will be UTF-8 encoded and represented as a hexidecimal value.";
        var tsExpectedResult = "546869732069732061207465787420746861742077696C6C206265205554462D3820656E636F64656420616E6420726570726573656E74656420617320612068657869646563696D616C2076616C75652E".ToLowerInvariant();
        var tsx = StringFactory.CreateHexadecimal(ts);
        var tsxByteArray = ParserFactory.FromHexadecimal().Parse(tsx);
        var tsxReverse = Decorator.Enclose(tsxByteArray).ToEncodedString();

        Assert.Equal(ts, tsxReverse);
        Assert.Equal(tsExpectedResult, tsx);
    }

    [Fact]
    public void ParserFactory_ShouldConvertUriSchemeString_ToUriScheme()
    {
        var http = ParserFactory.FromUriScheme().Parse("http");
        var https = ParserFactory.FromUriScheme().Parse("https");
        var ftp = ParserFactory.FromUriScheme().Parse("ftp");
        var sftp = ParserFactory.FromUriScheme().Parse("sftp");
        var netTcp = ParserFactory.FromUriScheme().Parse("net.TCP");

        Assert.Equal(UriScheme.Http, http);
        Assert.Equal(UriScheme.Https, https);
        Assert.Equal(UriScheme.Sftp, sftp);
        Assert.Equal(UriScheme.NetTcp, netTcp);
        Assert.Equal(UriScheme.Ftp, ftp);

        Assert.Equal("http", StringFactory.CreateUriScheme(http));
        Assert.Equal("https", StringFactory.CreateUriScheme(https));
        Assert.Equal("ftp", StringFactory.CreateUriScheme(ftp));
        Assert.Equal("sftp", StringFactory.CreateUriScheme(sftp));
        Assert.Equal("net.tcp", StringFactory.CreateUriScheme(netTcp));
    }

    [Fact]
    public void ParserFactory_ShouldConvertSimpleValueString_ToValueType()
    {
        var bo = ParserFactory.FromValueType().Parse("true");
        var by = ParserFactory.FromValueType().Parse("127");
        var i = ParserFactory.FromValueType().Parse("55465465");
        var lo = ParserFactory.FromValueType().Parse("165461116544564");
        var d = ParserFactory.FromValueType().Parse("1132313.33", o => o.FormatProvider = CultureInfo.InvariantCulture);
        var dt = ParserFactory.FromValueType().Parse("2020-05-12");
        var g = ParserFactory.FromValueType().Parse("92713ADE-6309-465C-A898-3270EFF2FE63");

        Assert.IsType<bool>(bo);
        Assert.Equal(true, bo);

        Assert.IsType<byte>(by);
        Assert.Equal((byte)127, by);

        Assert.IsType<int>(i);
        Assert.Equal(55465465, i);

        Assert.IsType<long>(lo);
        Assert.Equal(165461116544564, lo);

        Assert.IsType<double>(d);
        Assert.Equal(1132313.33, d);

        Assert.IsType<DateTime>(dt);
        Assert.Equal(new DateTime(2020, 5, 12), dt);

        Assert.IsType<Guid>(g);
        Assert.Equal(ParserFactory.FromGuid().Parse("92713ADE-6309-465C-A898-3270EFF2FE63"), g);
    }

    [Fact]
    public void ParserFactory_ShouldConvertUriString_ToUri()
    {
        var cuemon = "https://www.cuemon.net/";
        Assert.Equal(new Uri("https://www.cuemon.net/"), ParserFactory.FromUri().Parse(cuemon));
        Assert.Throws<InvalidEnumArgumentException>(() => ParserFactory.FromUri().Parse(cuemon, o =>
        {
            o.Schemes.Clear();
            o.Schemes.Add((UriScheme)42);
        }));
        Assert.Throws<ArgumentException>(() => ParserFactory.FromUri().Parse("a" + cuemon));
    }

    [Fact]
    public void ParserFactory_ShouldConvertEnumString_ToEnum()
    {
        Assert.Equal(AttributeTargets.Assembly, ParserFactory.FromEnum().Parse<AttributeTargets>("1"));
        Assert.Equal(AttributeTargets.Assembly, ParserFactory.FromEnum().Parse<AttributeTargets>("Assembly"));
        Assert.Equal(AttributeTargets.ReturnValue, ParserFactory.FromEnum().Parse("ReturnValue", typeof(AttributeTargets)));
        Assert.Throws<ArgumentException>(() => ParserFactory.FromEnum().Parse<AttributeTargets>("assembly", o => o.IgnoreCase = false));
        Assert.Throws<ArgumentException>(() => ParserFactory.FromEnum().Parse<VerticalDirection>("2"));
        Assert.Throws<ArgumentException>(() => ParserFactory.FromEnum().Parse<VerticalDirection>("Invalid"));
        Assert.Throws<OverflowException>(() => ParserFactory.FromEnum().Parse<VerticalDirection>($"{long.MaxValue}"));
    }
}
