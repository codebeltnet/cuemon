using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class CacheableOptionsTest : Test
{
    public CacheableOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CacheableOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new CacheableOptions()
        {
            Validators = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Validators == null')", sut2.Message);
        Assert.Equal("CacheableOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void CacheableOptions_ShouldHaveDefaultValues()
    {
        var sut = new CacheableOptions();

        Assert.NotNull(sut.Validators);
        Assert.NotNull(sut.CacheControl);
        Assert.NotNull(sut.Expires);
        Assert.True(sut.UseCacheControl);
        Assert.True(sut.UseExpires);
    }
}
