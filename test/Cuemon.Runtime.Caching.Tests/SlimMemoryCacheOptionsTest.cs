using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Runtime.Caching;
public class SlimMemoryCacheOptionsTest : Test
{
    public SlimMemoryCacheOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SlimMemoryCacheOptions_ShouldThrowArgumentNullException_ForClientFactory()
    {
        var sut1 = new SlimMemoryCacheOptions()
        {
            KeyProvider = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'KeyProvider == null')", sut2.Message);
        Assert.StartsWith("SlimMemoryCacheOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void SlimMemoryCacheOptions_ShouldHaveDefaultValues()
    {
        var sut = new SlimMemoryCacheOptions();

        Assert.NotNull(sut.KeyProvider);
        Assert.True(sut.EnableCleanup);
        Assert.Equal(TimeSpan.FromSeconds(30), sut.FirstSweep);
        Assert.Equal(TimeSpan.FromMinutes(2), sut.SucceedingSweep);
    }
}
