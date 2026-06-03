using Cuemon.Extensions;
using Cuemon.Extensions.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data.Integrity;

public class ChecksumBuilderTest : Test
{
    public ChecksumBuilderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameChecksum()
    {
        var first = 0d.FromUnixEpochTime().GetCacheValidator();
        _ = first.Checksum;
        var second = first.Clone();

        Assert.True(first.Equals(second));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentChecksum()
    {
        var first = 0d.FromUnixEpochTime().GetCacheValidator();
        var second = 1d.FromUnixEpochTime().GetCacheValidator();

        Assert.False(first.Equals(second));
    }

    [Fact]
    public void GetHashCode_ShouldReturnConsistentValue()
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();

        var first = sut.GetHashCode();
        var second = sut.GetHashCode();

        Assert.Equal(first, second);
    }

    [Fact]
    public void ToString_ShouldReturnHexString()
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();
        var result = sut.ToString();

        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Matches("^[0-9a-f]+$", result);
        TestOutput.WriteLine(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenObjectIsNotChecksumBuilder()
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();

        Assert.False(sut.Equals(new object()));
    }
}
