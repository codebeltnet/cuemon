using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
public class HttpCacheableOptionsTest : Test
{
    public HttpCacheableOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HttpCacheableOptions_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HttpCacheableOptions();
        sut1.Filters = null;
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Filters == null')", sut2.Message);
        Assert.Equal("HttpCacheableOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpCacheableOptions_ShouldHaveDefaultValues()
    {
        var sut = new HttpCacheableOptions();

        Assert.NotNull(sut.Filters);
        Assert.NotNull(sut.CacheControl);
        Assert.True(sut.UseCacheControl);
    }
}
