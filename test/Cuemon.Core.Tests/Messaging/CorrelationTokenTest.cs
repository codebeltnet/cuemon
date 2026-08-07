using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Messaging;
public class CorrelationTokenTest : Test
{
    public CorrelationTokenTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CorrelationId_ShouldHaveDefault32DigitsGuid()
    {
        var sut = new CorrelationToken();
        Assert.True(Guid.TryParse(sut.CorrelationId, out _));
        Assert.Equal(32, sut.CorrelationId.Length);
    }

    [Fact]
    public void CorrelationId_ShouldBeUniquePerInstance()
    {
        var sut = Generate.RangeOf(100, _ => new CorrelationToken()).ToList();

        Assert.Equal(100, sut.Select(ct => ct.CorrelationId).Distinct().Count());
        Assert.Equal(100, sut.Count);
    }

    [Fact]
    public void CorrelationId_ShouldTakeProvidedValue()
    {
        var expected = Generate.RandomString(24);
        var sut = new CorrelationToken(expected);

        Assert.Equal(expected, sut.ToString());
        Assert.Equal(expected, sut.CorrelationId);
        Assert.Equal(24, sut.CorrelationId.Length);
    }
}
