using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net.Http;
public class HttpManagerOptionsTest : Test
{
    public HttpManagerOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HttpManagerOptions_ShouldThrowArgumentNullException_ForDefaultRequestHeaders()
    {
        var sut1 = new HttpManagerOptions
        {
            DefaultRequestHeaders = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'DefaultRequestHeaders == null')", sut2.Message);
        Assert.StartsWith("HttpManagerOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpManagerOptions_ShouldThrowArgumentNullException_ForHandlerFactory()
    {
        var sut1 = new HttpManagerOptions
        {
            HandlerFactory = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'HandlerFactory == null')", sut2.Message);
        Assert.StartsWith("HttpManagerOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpManagerOptions_ShouldHaveDefaultValues()
    {
        var sut = new HttpManagerOptions();

        Assert.NotNull(sut.DefaultRequestHeaders);
        Assert.NotNull(sut.HandlerFactory);
        Assert.False(sut.DisposeHandler);
        Assert.Equal(TimeSpan.FromMinutes(2), sut.Timeout);
    }
}
