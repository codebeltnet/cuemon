using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class DecoratorTest : Test
{
    public DecoratorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Enclose_ShouldWrapValue()
    {
        var value = "cuemon";

        var sut = Decorator.Enclose(value);

        Assert.IsAssignableFrom<IDecorator<string>>(sut);
        Assert.Equal(value, sut.Inner);
        Assert.Null(sut.ArgumentName);
    }

    [Fact]
    public void Enclose_ShouldThrowArgumentNullException_WhenInnerIsNull()
    {
        string value = null;

        var ex = Assert.Throws<ArgumentNullException>(() => Decorator.Enclose(value));

        Assert.Equal("inner", ex.ParamName);
        Assert.Contains("Value cannot be null.", ex.Message);
    }

    [Fact]
    public void Enclose_ShouldAllowNull_WhenThrowIfNullIsFalse()
    {
        string value = null;

        var sut = Decorator.Enclose(value, false);

        Assert.Null(sut.Inner);
        Assert.Null(sut.ArgumentName);
    }

    [Fact]
    public void RawEnclose_ShouldWrapValueWithoutNullCheck()
    {
        string value = null;

        var sut = Decorator.RawEnclose(value);

        Assert.Null(sut.Inner);
        Assert.Null(sut.ArgumentName);
    }

    [Fact]
    public void EncloseToExpose_ShouldCaptureArgumentName()
    {
        var value = "cuemon";

        var sut = Decorator.EncloseToExpose(value);

        Assert.Equal(value, sut.Inner);
        Assert.Equal(nameof(value), sut.ArgumentName);
    }

    [Fact]
    public void EncloseToExpose_ShouldThrowArgumentNullException_WithCapturedArgumentName()
    {
        string value = null;

        var ex = Assert.Throws<ArgumentNullException>(() => Decorator.EncloseToExpose(value));

        Assert.Equal(nameof(value), ex.ParamName);
        Assert.Contains("Value cannot be null.", ex.Message);
    }

    [Fact]
    public void EncloseToExpose_ShouldAllowNull_WhenThrowIfNullIsFalse()
    {
        string value = null;

        var sut = Decorator.EncloseToExpose(value, false);

        Assert.Null(sut.Inner);
        Assert.Equal(nameof(value), sut.ArgumentName);
    }

    [Fact]
    public void Syntactic_ShouldReturnDecoratorWithDefaultInner()
    {
        var sut = Decorator.Syntactic<string>();

        Assert.Null(sut.Inner);
        Assert.Null(sut.ArgumentName);
    }

    [Fact]
    public void Syntactic_ShouldReturnDecoratorWithDefaultValueTypeInner()
    {
        var sut = Decorator.Syntactic<int>();

        Assert.Equal(default, sut.Inner);
        Assert.Null(sut.ArgumentName);
    }
}
