using System.IO;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Cuemon.Security;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Data.Integrity;
public class CacheValidatorExtensionsTest : Test
{
    public CacheValidatorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEntityTag_ShouldConvertCacheValidatorToEntityTag_Weak()
    {
        var sut1 = CacheValidatorFactory.CreateValidator(typeof(CacheValidatorExtensionsTest).Assembly);
        var sut2 = sut1.ToEntityTagHeaderValue();
        var sut3 = Generate.HashCode64(typeof(CacheValidatorExtensionsTest).Assembly.Location);
        var sut4 = HashFactory.CreateFnv64().ComputeHash(Convertible.GetBytes(sut3)).ToHexadecimalString();

        TestOutput.WriteLine(sut4);
        TestOutput.WriteLine(sut2.ToString());

        Assert.True(sut2.IsWeak);
        Assert.Equal<string>($"\"{sut4}\"", sut2.Tag.ToString());
        Assert.Equal($"W/\"{sut4}\"", sut2.ToString());
    }

    [Fact]
    public void ToEntityTag_ShouldConvertCacheValidatorToEntityTag_Strong()
    {
        var sut1 = CacheValidatorFactory.CreateValidator(typeof(CacheValidatorExtensionsTest).Assembly, setup: o => o.BytesToRead = int.MaxValue);
        var sut2 = sut1.ToEntityTagHeaderValue();
        var sut3 = File.ReadAllBytes(typeof(CacheValidatorExtensionsTest).Assembly.Location);
        var sut4 = HashFactory.CreateCrc64().ComputeHash(sut3).ToHexadecimalString();

        TestOutput.WriteLine(sut4);
        TestOutput.WriteLine(sut2.ToString());

        Assert.False(sut2.IsWeak);
        Assert.Equal<string>($"\"{sut4}\"", sut2.Tag.ToString());
        Assert.Equal($"\"{sut4}\"", sut2.ToString());
    }
}
