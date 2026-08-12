using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Hosting;
public class HostingEnvironmentOptionsTest : Test
{
    public HostingEnvironmentOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HostingEnvironmentOptions_HeaderNameIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HostingEnvironmentOptions
        {
            HeaderName = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("HostingEnvironmentOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HostingEnvironmentOptions_HeaderNameIsEmpty_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HostingEnvironmentOptions
        {
            HeaderName = string.Empty
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("HostingEnvironmentOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HostingEnvironmentOptions_HeaderNameIsWithWhitespaceOnly_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HostingEnvironmentOptions
        {
            HeaderName = " "
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName)')", sut2.Message);
        Assert.Equal("HostingEnvironmentOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HostingEnvironmentOptions_SuppressHeaderPredicateIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new HostingEnvironmentOptions()
        {
            SuppressHeaderPredicate = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SuppressHeaderPredicate == null')", sut2.Message);
        Assert.Equal("HostingEnvironmentOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void HostingEnvironmentOptions_ShouldHaveDefaultValues()
    {
        var sut = new HostingEnvironmentOptions();

        Assert.Equal("X-Hosting-Environment", sut.HeaderName);
        Assert.NotNull(sut.SuppressHeaderPredicate);
    }
}
