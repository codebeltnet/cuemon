using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class ExceptionConditionTest : Test
{
    public ExceptionConditionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IsTrue_ShouldThrowArgumentNullException_WhenConditionIsNull()
    {
        var sut = new ExceptionCondition<InvalidOperationException>();

        Assert.Throws<ArgumentNullException>(() => sut.IsTrue((Func<bool>)null));
        Assert.Throws<ArgumentNullException>(() => sut.IsTrue<string>(null));
    }

    [Fact]
    public void IsFalse_ShouldThrowArgumentNullException_WhenConditionIsNull()
    {
        var sut = new ExceptionCondition<InvalidOperationException>();

        Assert.Throws<ArgumentNullException>(() => sut.IsFalse((Func<bool>)null));
        Assert.Throws<ArgumentNullException>(() => sut.IsFalse<string>(null));
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenHandlerIsNull()
    {
        var sut = new ExceptionCondition<InvalidOperationException>();
        TesterFunc<string, bool> condition = (out string result) =>
        {
            result = "value";
            return true;
        };

        Assert.Throws<ArgumentNullException>(() => sut.IsTrue(() => true).Create(null));
        Assert.Throws<ArgumentNullException>(() => sut.IsTrue(condition).Create(null));
    }

    [Fact]
    public void TryThrow_ShouldThrow_WhenIsTrueConditionMatchesExpectedValue()
    {
        var invoked = false;
        var sut = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(() => true)
            .Create(() =>
            {
                invoked = true;
                return new InvalidOperationException("boom");
            });

        var ex = Assert.Throws<InvalidOperationException>(() => sut.TryThrow());

        Assert.True(invoked);
        Assert.Equal("boom", ex.Message);
    }

    [Fact]
    public void TryThrow_ShouldNotThrow_WhenIsTrueConditionDoesNotMatchExpectedValue()
    {
        var invoked = false;
        var sut = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(() => false)
            .Create(() =>
            {
                invoked = true;
                return new InvalidOperationException();
            });

        sut.TryThrow();

        Assert.False(invoked);
    }

    [Fact]
    public void TryThrow_ShouldThrow_WhenIsFalseConditionMatchesExpectedValue()
    {
        var invoked = false;
        var sut = new ExceptionCondition<ArgumentException>()
            .IsFalse(() => false)
            .Create(() =>
            {
                invoked = true;
                return new ArgumentException("boom");
            });

        var ex = Assert.Throws<ArgumentException>(() => sut.TryThrow());

        Assert.True(invoked);
        Assert.Equal("boom", ex.Message);
    }

    [Fact]
    public void TryThrow_ShouldNotThrow_WhenIsFalseConditionDoesNotMatchExpectedValue()
    {
        var invoked = false;
        var sut = new ExceptionCondition<ArgumentException>()
            .IsFalse(() => true)
            .Create(() =>
            {
                invoked = true;
                return new ArgumentException();
            });

        sut.TryThrow();

        Assert.False(invoked);
    }

    [Fact]
    public void TryThrow_ShouldPassTesterResultToHandler_WhenIsTrueMatchesExpectedValue()
    {
        var invoked = false;
        TesterFunc<string, bool> condition = (out string result) =>
        {
            result = "value";
            return true;
        };

        var sut = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(condition)
            .Create(result =>
            {
                invoked = true;
                return new InvalidOperationException(result);
            });

        var ex = Assert.Throws<InvalidOperationException>(() => sut.TryThrow());

        Assert.True(invoked);
        Assert.Equal("value", ex.Message);
    }

    [Fact]
    public void TryThrow_ShouldPassTesterResultToHandler_WhenIsFalseMatchesExpectedValue()
    {
        var invoked = false;
        TesterFunc<string, bool> condition = (out string result) =>
        {
            result = "value";
            return false;
        };

        var sut = new ExceptionCondition<InvalidOperationException>()
            .IsFalse(condition)
            .Create(result =>
            {
                invoked = true;
                return new InvalidOperationException(result);
            });

        var ex = Assert.Throws<InvalidOperationException>(() => sut.TryThrow());

        Assert.True(invoked);
        Assert.Equal("value", ex.Message);
    }

    [Fact]
    public void TryThrow_ShouldNotInvokeHandler_WhenTesterConditionDoesNotMatchExpectedValue()
    {
        var isTrueInvoked = false;
        var isFalseInvoked = false;
        TesterFunc<string, bool> isTrueCondition = (out string result) =>
        {
            result = "value";
            return false;
        };
        TesterFunc<string, bool> isFalseCondition = (out string result) =>
        {
            result = "value";
            return true;
        };

        var isTrue = new ExceptionCondition<InvalidOperationException>()
            .IsTrue(isTrueCondition)
            .Create(result =>
            {
                isTrueInvoked = true;
                return new InvalidOperationException(result);
            });

        var isFalse = new ExceptionCondition<InvalidOperationException>()
            .IsFalse(isFalseCondition)
            .Create(result =>
            {
                isFalseInvoked = true;
                return new InvalidOperationException(result);
            });

        isTrue.TryThrow();
        isFalse.TryThrow();

        Assert.False(isTrueInvoked);
        Assert.False(isFalseInvoked);
    }
}
