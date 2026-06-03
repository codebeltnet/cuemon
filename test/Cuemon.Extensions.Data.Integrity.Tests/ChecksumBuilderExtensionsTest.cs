using System;
using System.IO;
using Cuemon.Data.Integrity;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data.Integrity;

public class ChecksumBuilderExtensionsTest : Test
{
    public ChecksumBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsDouble()
    {
        AssertChecksumChanges(sut => sut.CombineWith(3.14d));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsShort()
    {
        AssertChecksumChanges(sut => sut.CombineWith((short)42));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsString()
    {
        AssertChecksumChanges(sut => sut.CombineWith("cuemon"));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsInt()
    {
        AssertChecksumChanges(sut => sut.CombineWith(42));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsLong()
    {
        AssertChecksumChanges(sut => sut.CombineWith(42L));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsFloat()
    {
        AssertChecksumChanges(sut => sut.CombineWith(3.14f));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsUShort()
    {
        AssertChecksumChanges(sut => sut.CombineWith((ushort)42));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsUInt()
    {
        AssertChecksumChanges(sut => sut.CombineWith(42U));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsULong()
    {
        AssertChecksumChanges(sut => sut.CombineWith(42UL));
    }

    [Fact]
    public void CombineWith_ShouldChangeChecksum_WhenAdditionalChecksumIsByteArray()
    {
        AssertChecksumChanges(sut => ChecksumBuilderExtensions.CombineWith(sut, new byte[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void CombineWith_ShouldReturnSameInstanceUnchanged_WhenByteArrayIsNull()
    {
        AssertChecksumUnchanged((byte[])null);
    }

    [Fact]
    public void CombineWith_ShouldReturnSameInstanceUnchanged_WhenByteArrayIsEmpty()
    {
        AssertChecksumUnchanged(Array.Empty<byte>());
    }

    private void AssertChecksumChanges(Func<CacheValidator, CacheValidator> combine)
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();
        var original = sut.ToString();

        var result = combine(sut);

        Assert.NotNull(result);
        Assert.Same(sut, result);
        Assert.NotEqual(original, result.ToString());
        TestOutput.WriteLine(result.ToString());
    }

    private void AssertChecksumUnchanged(byte[] additionalChecksum)
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();
        var original = sut.ToString();

        var result = ChecksumBuilderExtensions.CombineWith(sut, additionalChecksum);

        Assert.NotNull(result);
        Assert.Same(sut, result);
        Assert.Equal(original, result.ToString());
        TestOutput.WriteLine(result.ToString());
    }
}
