using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Resilience;
public class TransientOperationOptionsTest : Test
{
    public TransientOperationOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TransientOperationOptions_ShouldThrowArgumentNullException_ForRetryStrategy()
    {
        var sut1 = new TransientOperationOptions()
        {
            RetryStrategy = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'RetryStrategy == null')", sut2.Message);
        Assert.StartsWith("TransientOperationOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void TransientOperationOptions_ShouldThrowArgumentNullException_ForDetectionStrategy()
    {
        var sut1 = new TransientOperationOptions()
        {
            DetectionStrategy = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'DetectionStrategy == null')", sut2.Message);
        Assert.StartsWith("TransientOperationOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void TransientOperationOptions_ShouldHaveDefaultValues()
    {
        var sut = new TransientOperationOptions();

        Assert.NotNull(sut.DetectionStrategy);
        Assert.NotNull(sut.RetryStrategy);
        Assert.Equal(TransientOperationOptions.DefaultRetryAttempts, sut.RetryAttempts);
        Assert.True(sut.EnableRecovery);
        Assert.Equal(TimeSpan.FromMinutes(2), sut.MaximumAllowedLatency);
    }
}
