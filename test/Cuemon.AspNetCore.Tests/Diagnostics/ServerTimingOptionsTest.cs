using System;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class ServerTimingOptionsTest : Test
{
    public ServerTimingOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ServerTimingOptions_ShouldThrowArgumentNullException_ForSuppressHeaderPredicate()
    {
        var sut1 = new ServerTimingOptions
        {
            SuppressHeaderPredicate = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SuppressHeaderPredicate == null')", sut2.Message);
        Assert.Equal("ServerTimingOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ServerTimingOptions_ShouldHaveDefaultValues()
    {
        var sut = new ServerTimingOptions();

        Assert.Null(sut.MethodDescriptor);
        Assert.Null(sut.RuntimeParameters);
        Assert.Equal(TimeMeasureOptions.DefaultTimeMeasureCompletedThreshold, sut.TimeMeasureCompletedThreshold);
        Assert.NotNull(sut.LogLevelSelector);
        Assert.NotNull(sut.SuppressHeaderPredicate);
    }
}
