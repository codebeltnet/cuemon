using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class IntegerExtensionsTest : Test
{
    public IntegerExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Min_Int32_ShouldReturnTheSmallestValue()
    {
        int sut1 = 1;
        int sut2 = 100;

        Assert.Equal(sut1, sut1.Min(sut2));
    }

    [Fact]
    public void Min_Int64_ShouldReturnTheSmallestValue()
    {
        long sut1 = 1;
        long sut2 = 100;

        Assert.Equal(sut1, sut1.Min(sut2));
    }

    [Fact]
    public void Min_Int16_ShouldReturnTheSmallestValue()
    {
        short sut1 = 1;
        short sut2 = 100;

        Assert.Equal(sut1, sut1.Min(sut2));
    }

    [Fact]
    public void Max_Int32_ShouldReturnTheSmallestValue()
    {
        int sut1 = 1;
        int sut2 = 100;

        Assert.Equal(sut2, sut1.Max(sut2));
    }

    [Fact]
    public void Max_Int64_ShouldReturnTheSmallestValue()
    {
        long sut1 = 1;
        long sut2 = 100;

        Assert.Equal(sut2, sut1.Max(sut2));
    }

    [Fact]
    public void Max_Int16_ShouldReturnTheSmallestValue()
    {
        short sut1 = 1;
        short sut2 = 100;

        Assert.Equal(sut2, sut1.Max(sut2));
    }

    [Fact]
    public void IsPrime_NumbersShouldBeNatural()
    {
        var sut1 = Enumerable.Range(1, 20);

        Assert.Collection(sut1,
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()),
            i => Assert.True(i.IsPrime()),
            i => Assert.False(i.IsPrime()));
    }

    [Fact]
    public void IsCountableSequence_Int32_ShouldBeTrueWhenSequenceIsIncrementedOrDecrementedBySameCardinality()
    {
        var sut1 = Enumerable.Range(1, 10);
        var sut2 = Enumerable.Range(-9, 10);
        var sut3 = Generate.RangeOf(10, i => Generate.RandomNumber(i, 100000));
        var sut4 = Generate.RangeOf(10, i => Generate.RandomNumber(i, 100000) * -1);
        var sut5 = Generate.RangeOf(10, i => i * 2);
        var sut6 = Generate.RangeOf(10, i => i * 3);
        var sut7 = Generate.RangeOf(10, i => i * 4);

        Assert.True(sut1.IsCountableSequence());
        Assert.True(sut2.IsCountableSequence());
        Assert.False(sut3.IsCountableSequence());
        Assert.False(sut4.IsCountableSequence());
        Assert.True(sut5.IsCountableSequence());
        Assert.True(sut6.IsCountableSequence());
        Assert.True(sut7.IsCountableSequence());
    }

    [Fact]
    public void IsCountableSequence_Int64_ShouldBeTrueWhenSequenceIsIncrementedOrDecrementedBySameCardinality()
    {
        var sut1 = Generate.RangeOf<long>(10, i => i);
        var sut2 = Generate.RangeOf<long>(10, i => i * -1);
        var sut3 = Generate.RangeOf<long>(10, i => Generate.RandomNumber(i, 100000));
        var sut4 = Generate.RangeOf<long>(10, i => Generate.RandomNumber(i, 100000) * -1);
        var sut5 = Generate.RangeOf<long>(10, i => i * 2);
        var sut6 = Generate.RangeOf<long>(10, i => i * 3);
        var sut7 = Generate.RangeOf<long>(10, i => i * 4);

        Assert.True(sut1.IsCountableSequence());
        Assert.True(sut2.IsCountableSequence());
        Assert.False(sut3.IsCountableSequence());
        Assert.False(sut4.IsCountableSequence());
        Assert.True(sut5.IsCountableSequence());
        Assert.True(sut6.IsCountableSequence());
        Assert.True(sut7.IsCountableSequence());
    }

    [Fact]
    public void IsEven_ShouldBeEven()
    {
        var sut1 = Enumerable.Range(1, 20);

        TestOutput.WriteLines(sut1.Cast<object>().ToArray());

        Assert.Collection(sut1,
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()),
            i => Assert.False(i.IsEven()),
            i => Assert.True(i.IsEven()));
    }

    [Fact]
    public void IsOdd_ShouldBeEven()
    {
        var sut1 = Enumerable.Range(2, 20);

        TestOutput.WriteLines(sut1);

        Assert.Collection(sut1,
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()),
            i => Assert.False(i.IsOdd()),
            i => Assert.True(i.IsOdd()));
    }
}
