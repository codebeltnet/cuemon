using System.IO;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Collections.Generic;
public class ReferenceComparerTest : Test
{
    public ReferenceComparerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Default_ShouldBeGreaterThanX()
    {
        var x = new MemoryStream();
        var y = new object();
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeLessThanY()
    {
        var x = new object();
        var y = new MemoryStream();
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(-1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeEqualToX()
    {
        var x = new object();
        var y = new object();
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(0, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeGreaterThanXWhenYIsNull()
    {
        var x = new object();
        var y = default(object);
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeLessThanYWhenXIsNull()
    {
        var x = default(object);
        var y = new object();
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(-1, comparer.Compare(x, y));
    }

    [Fact]
    public void Default_ShouldBeEqualToXWhenBothAreNull()
    {
        var x = default(object);
        var y = default(object);
        var comparer = ReferenceComparer<object>.Default;
        Assert.Equal(0, comparer.Compare(x, y));
    }
}
