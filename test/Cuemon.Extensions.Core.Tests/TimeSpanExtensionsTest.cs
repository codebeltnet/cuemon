using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class TimeSpanExtensionsTest : Test
{
    public TimeSpanExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetTotalNanoseconds_ShouldConvertTicksToNanoseconds()
    {
        var ts = TimeSpan.FromHours(1);
        Assert.Equal(3.6E+12, ts.GetTotalNanoseconds());
    }

    [Fact]
    public void GetTotalMicroseconds_ShouldConvertTicksToMicroseconds()
    {
        var ts = TimeSpan.FromHours(1);
        Assert.Equal(3.6E+9, ts.GetTotalMicroseconds());
    }

    [Fact]
    public void Floor_ShouldRoundDownToNearestUnit()
    {
        var ts1 = TimeSpan.FromMinutes(280);
        var ts2 = TimeSpan.FromMinutes(45);

        Assert.Equal(ts1.Floor(1, TimeUnit.Hours), ts1.Floor(TimeSpan.FromHours(1)));
        Assert.Equal(TimeSpan.FromHours(4), ts1.Floor(1, TimeUnit.Hours));

        Assert.Equal(ts2.Floor(1, TimeUnit.Hours), ts2.Floor(TimeSpan.FromHours(1)));
        Assert.Equal(TimeSpan.FromHours(0), ts2.Floor(1, TimeUnit.Hours));
    }

    [Fact]
    public void Ceiling_ShouldRoundUpToNearestUnit()
    {
        var ts1 = TimeSpan.FromMinutes(280);
        var ts2 = TimeSpan.FromMinutes(45);

        Assert.Equal(ts1.Ceiling(1, TimeUnit.Hours), ts1.Ceiling(TimeSpan.FromHours(1)));
        Assert.Equal(TimeSpan.FromHours(5), ts1.Ceiling(1, TimeUnit.Hours));

        Assert.Equal(ts2.Ceiling(1, TimeUnit.Hours), ts2.Ceiling(TimeSpan.FromHours(1)));
        Assert.Equal(TimeSpan.FromHours(1), ts2.Ceiling(1, TimeUnit.Hours));
    }
}
