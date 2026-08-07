using System;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;
using Cuemon.Extensions;

namespace Cuemon.Extensions;
public class DoubleExtensionsTest : Test
{
    public DoubleExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void FromUnixEpochTime_ShouldConvertToDoubleUnixEpochTime()
    {
        var sut1 = 1617738277d;
        var sut2 = sut1.FromUnixEpochTime().ToLocalTime();
        var sut3 = sut2.ToUnixEpochTime();

        TestOutput.WriteLine(sut2.ToString());

        Assert.Equal(sut1, sut3);
    }

    [Fact]
    public void ToTimeSpan_ShouldConvertDoubleToTimeSpan()
    {
        var sut1 = 1617738277d.ToTimeSpan(TimeUnit.Seconds);
        var sut2 = 864000000000d.ToTimeSpan(TimeUnit.Ticks);
        var sut3 = 1d.ToTimeSpan(TimeUnit.Days);


        Assert.Equal("18723.19:44:37", sut1.ToString());
        Assert.Equal("1.00:00:00", sut2.ToString());
        Assert.Equal(sut3, sut2);
    }

    [Fact]
    public void Factorial_ShouldCalculateSelectedSequence()
    {
        var sut1 = Generate.RangeOf(21, i => Convert.ToDouble(i).Factorial());

        Assert.Collection(sut1,
            d => Assert.Equal(1, d),
            d => Assert.Equal(1, d),
            d => Assert.Equal(2, d),
            d => Assert.Equal(6, d),
            d => Assert.Equal(24, d),
            d => Assert.Equal(120, d),
            d => Assert.Equal(720, d),
            d => Assert.Equal(5040, d),
            d => Assert.Equal(40320, d),
            d => Assert.Equal(362880, d),
            d => Assert.Equal(3628800, d),
            d => Assert.Equal(39916800, d),
            d => Assert.Equal(479001600, d),
            d => Assert.Equal(6227020800, d),
            d => Assert.Equal(87178291200, d),
            d => Assert.Equal(1307674368000, d),
            d => Assert.Equal(20922789888000, d),
            d => Assert.Equal(355687428096000, d),
            d => Assert.Equal(6402373705728000, d),
            d => Assert.Equal(121645100408832000, d),
            d => Assert.Equal(2432902008176640000, d));
    }

    [Fact]
    public void RoundOff_ShouldRoundOffToSpecifiedAccuracy()
    {
        var sut1 = 123456789.987654321d;
        var sut2 = sut1.RoundOff(RoundOffAccuracy.NearestTenth);
        var sut3 = sut1.RoundOff(RoundOffAccuracy.NearestHundredth);
        var sut4 = sut1.RoundOff(RoundOffAccuracy.NearestThousandth);
        var sut5 = sut1.RoundOff(RoundOffAccuracy.NearestTenThousandth);
        var sut6 = sut1.RoundOff(RoundOffAccuracy.NearestHundredThousandth);
        var sut7 = sut1.RoundOff(RoundOffAccuracy.NearestMillion);

        TestOutput.WriteLines(sut2, sut3, sut4, sut5, sut6, sut7);

        Assert.Equal(123456790, sut2);
        Assert.Equal(123456800, sut3);
        Assert.Equal(123457000, sut4);
        Assert.Equal(123460000, sut5);
        Assert.Equal(123500000, sut6);
        Assert.Equal(123000000, sut7);
    }
}
