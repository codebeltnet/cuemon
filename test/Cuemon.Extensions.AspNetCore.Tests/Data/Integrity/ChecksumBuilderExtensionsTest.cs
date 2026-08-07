using System.IO;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Cuemon.Security;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Data.Integrity;
public class ChecksumBuilderExtensionsTest : Test
{
    public ChecksumBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEntityTagHeaderValue_ShouldConvertCacheValidatorToEntityTag_Weak()
    {
        ChecksumBuilder sut1 = CacheValidatorFactory.CreateValidator(typeof(ChecksumBuilderExtensionsTest).Assembly);
        var sut2 = sut1.ToEntityTagHeaderValue(true);
        var sut3 = Generate.HashCode64(typeof(ChecksumBuilderExtensionsTest).Assembly.Location);
        var sut4 = HashFactory.CreateFnv64().ComputeHash(Convertible.GetBytes(sut3)).ToHexadecimalString();

        TestOutput.WriteLine(sut4);
        TestOutput.WriteLine(sut2.ToString());

        Assert.True(sut2.IsWeak);
        Assert.Equal<string>($"\"{sut4}\"", sut2.Tag.ToString());
        Assert.Equal($"W/\"{sut4}\"", sut2.ToString());
    }

    [Fact]
    public void ToEntityTagHeaderValue_ShouldConvertCacheValidatorToEntityTag_Strong()
    {
        ChecksumBuilder sut1 = CacheValidatorFactory.CreateValidator(typeof(ChecksumBuilderExtensionsTest).Assembly, setup: o => o.BytesToRead = int.MaxValue);
        var sut2 = sut1.ToEntityTagHeaderValue();
        var sut3 = File.ReadAllBytes(typeof(ChecksumBuilderExtensionsTest).Assembly.Location);
        var sut4 = HashFactory.CreateCrc64().ComputeHash(sut3).ToHexadecimalString();

        TestOutput.WriteLine(sut4);
        TestOutput.WriteLine(sut2.ToString());

        Assert.False(sut2.IsWeak);
        Assert.Equal<string>($"\"{sut4}\"", sut2.Tag.ToString());
        Assert.Equal($"\"{sut4}\"", sut2.ToString());
    }
}
