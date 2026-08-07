using System;
using Cuemon.AspNetCore.Http.Headers;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Http.Throttling;
public class ThrottlingSentinelOptionsTest : Test
{
    public ThrottlingSentinelOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitHeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitHeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitHeaderName) || Condition.IsEmpty(RateLimitHeaderName) || Condition.IsWhiteSpace(RateLimitHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitHeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitHeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitHeaderName) || Condition.IsEmpty(RateLimitHeaderName) || Condition.IsWhiteSpace(RateLimitHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitHeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitHeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitHeaderName) || Condition.IsEmpty(RateLimitHeaderName) || Condition.IsWhiteSpace(RateLimitHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitRemainingHeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitRemainingHeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitRemainingHeaderName) || Condition.IsEmpty(RateLimitRemainingHeaderName) || Condition.IsWhiteSpace(RateLimitRemainingHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitRemainingHeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitRemainingHeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitRemainingHeaderName) || Condition.IsEmpty(RateLimitRemainingHeaderName) || Condition.IsWhiteSpace(RateLimitRemainingHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitRemainingHeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitRemainingHeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitRemainingHeaderName) || Condition.IsEmpty(RateLimitRemainingHeaderName) || Condition.IsWhiteSpace(RateLimitRemainingHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitResetHeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitResetHeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitResetHeaderName) || Condition.IsEmpty(RateLimitResetHeaderName) || Condition.IsWhiteSpace(RateLimitResetHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitResetHeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitResetHeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitResetHeaderName) || Condition.IsEmpty(RateLimitResetHeaderName) || Condition.IsWhiteSpace(RateLimitResetHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_RateLimitResetHeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions
        {
            RateLimitResetHeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(RateLimitResetHeaderName) || Condition.IsEmpty(RateLimitResetHeaderName) || Condition.IsWhiteSpace(RateLimitResetHeaderName)')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_ContextResolverHasValueAndQuotaIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions()
        {
            ContextResolver = context => "FAKECONTEXT"
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ContextResolver != null && Quota == null')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_ResponseHandlerIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new ThrottlingSentinelOptions()
        {
            ResponseHandler = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ResponseHandler == null')", sut2.Message);
        Assert.Equal("ThrottlingSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ThrottlingSentinelOptions_ShouldHaveDefaultValues()
    {
        var sut = new ThrottlingSentinelOptions();

        Assert.Equal("RateLimit-Limit", sut.RateLimitHeaderName);
        Assert.Equal("RateLimit-Remaining", sut.RateLimitRemainingHeaderName);
        Assert.Equal("RateLimit-Reset", sut.RateLimitResetHeaderName);
        Assert.Equal("Throttling rate limit quota violation. Quota limit exceeded.", sut.TooManyRequestsMessage);
        Assert.Equal(RetryConditionScope.DeltaSeconds, sut.RateLimitResetScope);
        Assert.Equal(RetryConditionScope.DeltaSeconds, sut.RetryAfterScope);
        Assert.NotNull(sut.ResponseHandler);
        Assert.True(sut.UseRetryAfterHeader);
        Assert.Null(sut.ContextResolver);
        Assert.Null(sut.Quota);
    }
}
