using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Collections.Generic;
public class EnumerableSizeComparerTest : Test
{
    public EnumerableSizeComparerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Default_ShouldBeGreaterThanX()
    {
        var x = new[] { 1, 2, 3, 4, 5 };
        var y = new[] { 1, 2, 3, 4 };
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeLessThanY()
    {
        var x = new[] { 1, 2, 3, 4 };
        var y = new[] { 1, 2, 3, 4, 5 };
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(-1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeEqualToX()
    {
        var x = new[] { 1, 2, 3, 4 };
        var y = new[] { 1, 2, 3, 4 };
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(0, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeGreaterThanXWhenYIsNull()
    {
        var x = new[] { 1, 2, 3, 4 };
        var y = default(IEnumerable<int>);
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeLessThanYWhenXIsNull()
    {
        var x = default(IEnumerable<int>);
        var y = new[] { 1, 2, 3, 4 };
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(-1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeEqualToXWhenBothAreNull()
    {
        var x = default(IEnumerable<int>);
        var y = default(IEnumerable<int>);
        var comparer = EnumerableSizeComparer<IEnumerable<int>>.Default;
        Assert.Equal(0, comparer.Compare(x, y));
    }
}
