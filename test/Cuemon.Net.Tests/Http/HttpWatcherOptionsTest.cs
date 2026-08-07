using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net.Http;
public class HttpWatcherOptionsTest : Test
{
    public HttpWatcherOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HttpWatcherOptions_ShouldThrowArgumentNullException_ForClientFactory()
    {
        var sut1 = new HttpWatcherOptions
        {
            ClientFactory = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ClientFactory == null')", sut2.Message);
        Assert.StartsWith("HttpWatcherOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpWatcherOptions_ShouldThrowArgumentNullException_ForHashFactory()
    {
        var sut1 = new HttpWatcherOptions
        {
            HashFactory = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'HashFactory == null')", sut2.Message);
        Assert.StartsWith("HttpWatcherOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HttpWatcherOptions_ShouldHaveDefaultValues()
    {
        var sut = new HttpWatcherOptions();

        Assert.NotNull(sut.ClientFactory);
        Assert.NotNull(sut.HashFactory);
        Assert.False(sut.ReadResponseBody);
    }
}
