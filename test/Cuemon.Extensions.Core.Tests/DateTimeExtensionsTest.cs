using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class DateTimeExtensionsTest : Test
{
    public DateTimeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToUnixEpochTime_ShouldConvertToDoubleUnixEpochTime()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
        var sut2 = sut1.ToUnixEpochTime();
        var sut3 = sut2.FromUnixEpochTime().ToLocalTime();

        TestOutput.WriteLine(sut2.ToString());

        Assert.Equal(sut1, sut3);
    }

    [Fact]
    public void ToUtcKind_ShouldRepresentLocalTimeAsUtcWithoutConversion()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
        var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut3 = sut1.ToUtcKind();

        Assert.Equal(sut2, sut3);
    }

    [Fact]
    public void ToLocalKind_ShouldRepresentUtcTimeAsLocalWithoutConversion()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
        var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut3 = sut2.ToLocalKind();

        Assert.Equal(sut1, sut3);
    }

    [Fact]
    public void ToDefaultKind_ShouldRepresentUtcTimeAsDefaultWithoutConversion()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Local);
        var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut3 = sut1.ToDefaultKind();
        var sut4 = sut2.ToDefaultKind();

        Assert.Equal(sut3, sut4);
    }

    [Fact]
    public void IsWithinRange_DateShouldBeWithinSpecifiedRangeDate()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut2 = sut1.AddHours(-6);
        var sut3 = sut1.AddHours(6);
        var sut4 = sut1.IsWithinRange(sut2, sut3);
        var sut5 = sut1.IsWithinRange(sut2.AddHours(7), sut3.AddHours(-7));

        Assert.InRange(sut1, sut2, sut3);
        Assert.NotInRange(sut1, sut2.AddHours(7), sut3.AddHours(-7));
        Assert.True(sut4);
        Assert.False(sut5);
    }

    [Fact]
    public void IsWithinRange_DateShouldBeWithinSpecifiedRangeOfTime()
    {
        var sut1 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTimeRange(sut1.AddHours(-6), sut1.AddHours(6));
        var sut3 = sut1.IsWithinRange(sut2);
        var sut4 = sut1.IsWithinRange(sut2.Start.AddHours(7), sut2.End.AddHours(-7));

        Assert.InRange(sut1, sut2.Start, sut2.End);
        Assert.NotInRange(sut1, sut2.Start.AddHours(7), sut2.End.AddHours(-7));
        Assert.True(sut3);
        Assert.False(sut4);
    }

    [Fact]
    public void IsTimeOfDayNight_ShouldBeFrom2100To0300()
    {
        var sut1 = new DateTime(2021, 4, 6, 19, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);
        var sut3 = new DateTime(2021, 4, 6, 2, 44, 37, DateTimeKind.Utc);
        var sut4 = new DateTime(2021, 4, 6, 4, 44, 37, DateTimeKind.Utc);

        TestOutput.WriteLine(DayPart.Night.ToString());

        Assert.False(sut1.IsTimeOfDayNight());
        Assert.True(sut2.IsTimeOfDayNight());
        Assert.True(sut3.IsTimeOfDayNight());
        Assert.False(sut4.IsTimeOfDayNight());
    }

    [Fact]
    public void IsTimeOfDayMorning_ShouldBeFrom0300To0900()
    {
        var sut1 = new DateTime(2021, 4, 6, 2, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTime(2021, 4, 6, 4, 44, 37, DateTimeKind.Utc);
        var sut3 = new DateTime(2021, 4, 6, 8, 44, 37, DateTimeKind.Utc);
        var sut4 = new DateTime(2021, 4, 6, 10, 44, 37, DateTimeKind.Utc);

        TestOutput.WriteLine(DayPart.Morning.ToString());

        Assert.False(sut1.IsTimeOfDayMorning());
        Assert.True(sut2.IsTimeOfDayMorning());
        Assert.True(sut3.IsTimeOfDayMorning());
        Assert.False(sut4.IsTimeOfDayMorning());
    }

    [Fact]
    public void IsTimeOfDayForenoon_ShouldBeFrom0900To1200()
    {
        var sut1 = new DateTime(2021, 4, 6, 8, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTime(2021, 4, 6, 10, 44, 37, DateTimeKind.Utc);
        var sut3 = new DateTime(2021, 4, 6, 11, 44, 37, DateTimeKind.Utc);
        var sut4 = new DateTime(2021, 4, 6, 12, 44, 37, DateTimeKind.Utc);

        TestOutput.WriteLine(DayPart.Forenoon.ToString());

        Assert.False(sut1.IsTimeOfDayForenoon());
        Assert.True(sut2.IsTimeOfDayForenoon());
        Assert.True(sut3.IsTimeOfDayForenoon());
        Assert.False(sut4.IsTimeOfDayForenoon());
    }

    [Fact]
    public void IsTimeOfDayAfternoon_ShouldBeFrom1200To1800()
    {
        var sut1 = new DateTime(2021, 4, 6, 11, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTime(2021, 4, 6, 12, 44, 37, DateTimeKind.Utc);
        var sut3 = new DateTime(2021, 4, 6, 17, 44, 37, DateTimeKind.Utc);
        var sut4 = new DateTime(2021, 4, 6, 18, 44, 37, DateTimeKind.Utc);

        TestOutput.WriteLine(DayPart.Afternoon.ToString());

        Assert.False(sut1.IsTimeOfDayAfternoon());
        Assert.True(sut2.IsTimeOfDayAfternoon());
        Assert.True(sut3.IsTimeOfDayAfternoon());
        Assert.False(sut4.IsTimeOfDayAfternoon());
    }

    [Fact]
    public void IsTimeOfDayEvening_ShouldBeFrom1800To2100()
    {
        var sut1 = new DateTime(2021, 4, 6, 17, 44, 37, DateTimeKind.Utc);
        var sut2 = new DateTime(2021, 4, 6, 18, 44, 37, DateTimeKind.Utc);
        var sut3 = new DateTime(2021, 4, 6, 20, 44, 37, DateTimeKind.Utc);
        var sut4 = new DateTime(2021, 4, 6, 21, 44, 37, DateTimeKind.Utc);

        TestOutput.WriteLine(DayPart.Evening.ToString());

        Assert.False(sut1.IsTimeOfDayEvening());
        Assert.True(sut2.IsTimeOfDayEvening());
        Assert.True(sut3.IsTimeOfDayEvening());
        Assert.False(sut4.IsTimeOfDayEvening());
    }

    [Fact]
    public void Floor_ShouldRoundToNegativeInfinity()
    {
        var sut1 = new DateTime(2021, 4, 8, 21, 44, 37);
        var sut2 = sut1.Floor(TimeSpan.FromSeconds(10));
        var sut3 = sut1.Floor(TimeSpan.FromMinutes(15));
        var sut4 = sut1.Floor(TimeSpan.FromHours(1));
        var sut5 = sut1.Floor(TimeSpan.FromDays(1));
        var sut6 = sut1.Floor(10, TimeUnit.Seconds);
        var sut7 = sut1.Floor(15, TimeUnit.Minutes);
        var sut8 = sut1.Floor(1, TimeUnit.Hours);
        var sut9 = sut1.Floor(1, TimeUnit.Days);


        Assert.Equal("2021-04-08T21:44:30", sut2.ToString("s"));
        Assert.Equal("2021-04-08T21:30:00", sut3.ToString("s"));
        Assert.Equal("2021-04-08T21:00:00", sut4.ToString("s"));
        Assert.Equal("2021-04-08T00:00:00", sut5.ToString("s"));

        Assert.Equal(sut2, sut6);
        Assert.Equal(sut3, sut7);
        Assert.Equal(sut4, sut8);
        Assert.Equal(sut5, sut9);
    }

    [Fact]
    public void Ceiling_ShouldRoundTPositiveInfinity()
    {
        var sut1 = new DateTime(2021, 4, 8, 21, 44, 37);
        var sut2 = sut1.Ceiling(TimeSpan.FromSeconds(10));
        var sut3 = sut1.Ceiling(TimeSpan.FromMinutes(15));
        var sut4 = sut1.Ceiling(TimeSpan.FromHours(1));
        var sut5 = sut1.Ceiling(TimeSpan.FromDays(1));
        var sut6 = sut1.Ceiling(10, TimeUnit.Seconds);
        var sut7 = sut1.Ceiling(15, TimeUnit.Minutes);
        var sut8 = sut1.Ceiling(1, TimeUnit.Hours);
        var sut9 = sut1.Ceiling(1, TimeUnit.Days);


        Assert.Equal("2021-04-08T21:44:40", sut2.ToString("s"));
        Assert.Equal("2021-04-08T21:45:00", sut3.ToString("s"));
        Assert.Equal("2021-04-08T22:00:00", sut4.ToString("s"));
        Assert.Equal("2021-04-09T00:00:00", sut5.ToString("s"));

        Assert.Equal(sut2, sut6);
        Assert.Equal(sut3, sut7);
        Assert.Equal(sut4, sut8);
        Assert.Equal(sut5, sut9);
    }
}
