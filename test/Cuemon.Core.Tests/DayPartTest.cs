using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class DayPartTest : Test
{
    public DayPartTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Night_ShouldBeInRangeOfTwentyOneToThree()
    {
        var sut = DayPart.Night;

        Assert.Equal("Night", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(21));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(3));
    }

    [Fact]
    public void Morning_ShouldBeInRangeOfThreeToNine()
    {
        var sut = DayPart.Morning;

        Assert.Equal("Morning", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(3));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(9));
    }

    [Fact]
    public void Forenoon_ShouldBeInRangeOfNineToTwelve()
    {
        var sut = DayPart.Forenoon;

        Assert.Equal("Forenoon", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(3));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(9));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(12));
    }

    [Fact]
    public void Afternoon_ShouldBeInRangeOfTwelveToEighteen()
    {
        var sut = DayPart.Afternoon;

        Assert.Equal("Afternoon", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(6));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(12));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(18));
    }

    [Fact]
    public void Evening_ShouldBeInRangeOfEighteenToTwentyOne()
    {
        var sut = DayPart.Evening;

        Assert.Equal("Evening", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(3));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(18));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(21));
    }

    [Fact]
    public void Ctor_ShouldBeInRangeOfFourToSixInTheMorning()
    {
        var sut = new DayPart("Dawn", new TimeRange(TimeSpan.FromHours(4), TimeSpan.FromHours(6)));

        var s = new TimeRange(TimeSpan.FromHours(4), TimeSpan.FromHours(6));

        Assert.Equal("Dawn", sut.Name);
        Assert.Equal(sut.Range.Duration, TimeSpan.FromHours(2));
        Assert.Equal(sut.Range.Start, TimeSpan.FromHours(4));
        Assert.Equal(sut.Range.End, TimeSpan.FromHours(6));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenExceedingTwentyFourHoursDuration()
    {
        var sut = Assert.Throws<ArgumentOutOfRangeException>(() => new DayPart("Custom", new TimeRange(TimeSpan.Zero, TimeSpan.FromMilliseconds(86400001))));

        TestOutput.WriteLine(sut.ToString());
    }

    [Fact]
    public void Ctor_ShouldListAndVerifyAllBuiltInDayParts()
    {
        var sut = DayPart.All;

        Assert.Collection(sut,
            part => Assert.Equal(part.Name, nameof(DayPart.Night)),
            part => Assert.Equal(part.Name, nameof(DayPart.Morning)),
            part => Assert.Equal(part.Name, nameof(DayPart.Forenoon)),
            part => Assert.Equal(part.Name, nameof(DayPart.Afternoon)),
            part => Assert.Equal(part.Name, nameof(DayPart.Evening))
            );

        var all = DelimitedString.Create(sut.Select(part => part.ToString()), o => o.Delimiter = Environment.NewLine);

        TestOutput.WriteLine(all);
    }
}
