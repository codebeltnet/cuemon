using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Messaging;
public class RequestTokenTest : Test
{
    public RequestTokenTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void RequestToken_ShouldHaveDefault32DigitsGuid()
    {
        var sut = new RequestToken();
        Assert.True(Guid.TryParse(sut.RequestId, out _));
        Assert.Equal(32, sut.RequestId.Length);
    }

    [Fact]
    public void RequestToken_ShouldBeUniquePerInstance()
    {
        var sut = Generate.RangeOf(100, _ => new RequestToken()).ToList();

        Assert.Equal(100, sut.Select(rt => rt.RequestId).Distinct().Count());
        Assert.Equal(100, sut.Count);
    }

    [Fact]
    public void RequestToken_ShouldTakeProvidedValue()
    {
        var expected = Generate.RandomString(24);
        var sut = new RequestToken(expected);

        Assert.Equal(expected, sut.ToString());
        Assert.Equal(expected, sut.RequestId);
        Assert.Equal(24, sut.RequestId.Length);
    }
}
