using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class StringFactoryTest : Test
{
    public StringFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateHexadecimal_WithByteArrayNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringFactory.CreateHexadecimal((byte[])null));
    }

    [Fact]
    public void CreateHexadecimal_WithByteArray_ReturnsLowercaseHexadecimalString()
    {
        var sut = StringFactory.CreateHexadecimal(new byte[] { 0x0F, 0xA0, 0x01 });

        Assert.Equal("0fa001", sut);
    }

    [Fact]
    public void CreateHexadecimal_WithStringNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringFactory.CreateHexadecimal((string)null));
    }

    [Fact]
    public void CreateBinaryDigits_WithByteArrayNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringFactory.CreateBinaryDigits(null));
    }

    [Fact]
    public void CreateBinaryDigits_WithByteArray_ReturnsBinaryDigitString()
    {
        var sut = StringFactory.CreateBinaryDigits(new byte[] { 0, 1, 255 });

        Assert.Equal("000000000000000111111111", sut);
    }

    [Fact]
    public void CreateUrlEncodedBase64_WithByteArrayNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringFactory.CreateUrlEncodedBase64(null));
    }

    [Fact]
    public void CreateUrlEncodedBase64_WithByteArray_ReturnsUrlSafeBase64WithoutPadding()
    {
        var sut = StringFactory.CreateUrlEncodedBase64(new byte[] { 251, 255 });

        Assert.Equal("-_8", sut);
    }

    [Fact]
    public void CreateProtocolRelativeUrl_WithNullUri_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringFactory.CreateProtocolRelativeUrl(null));
    }

    [Fact]
    public void CreateProtocolRelativeUrl_WithRelativeUri_ThrowsArgumentException()
    {
        var sut = new Uri("/about", UriKind.Relative);

        Assert.Throws<ArgumentException>(() => StringFactory.CreateProtocolRelativeUrl(sut));
    }

    [Fact]
    public void CreateProtocolRelativeUrl_WithAbsoluteUri_ReturnsProtocolRelativeUrl()
    {
        var sut = StringFactory.CreateProtocolRelativeUrl(new Uri("https://www.cuemon.net/about"));

        Assert.Equal("//www.cuemon.net/about", sut);
    }

    [Fact]
    public void CreateUriScheme_WithKnownScheme_ReturnsUriSchemeName()
    {
        var sut = StringFactory.CreateUriScheme(UriScheme.Https);

        Assert.Equal("https", sut);
    }

    [Fact]
    public void CreateUriScheme_WithUnknownScheme_ReturnsUndefined()
    {
        var sut = StringFactory.CreateUriScheme((UriScheme)int.MaxValue);

        Assert.Equal(nameof(UriScheme.Undefined), sut);
    }
}
