using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class TimeBasedObjectResultOptionsTest : Test
{
    public TimeBasedObjectResultOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TimeBasedObjectResultOptions_ShouldThrowInvalidOperationExceptionWhenChecksumProviderIsNull()
    {
        var sut1 = new TimeBasedObjectResultOptions<DateTimeRange>();
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'TimestampProvider == null')", sut2.Message);
        Assert.Equal("TimeBasedObjectResultOptions<DateTimeRange> are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void TimeBasedObjectResultOptions_ShouldHaveDefaultValues()
    {
        var sut = new TimeBasedObjectResultOptions<DateTimeRange>();

        Assert.Null(sut.TimestampProvider);
        Assert.Null(sut.ChangedTimestampProvider);
    }
}
