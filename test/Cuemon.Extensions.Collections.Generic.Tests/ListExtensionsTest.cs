using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic;
public class ListExtensionsTest : Test
{
    public ListExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Remove_ShouldRemoveTheSpecifiedItemFromList()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = new List<bool>();

        for (var i = 1018; i < 1028; i++)
        {
            sut2.Add(sut1.Remove(item => item == i));
        }

        Assert.Equal(1018, sut1.Count);
        Assert.Collection(sut2,
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i));
    }

    [Fact]
    public void HasIndex_ShouldVerifyIfAnIndexIsValidWithinTheList()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = new List<bool>();

        for (var i = 1018; i < 1028; i++)
        {
            sut2.Add(sut1.HasIndex(i));
        }

        Assert.Equal(1024, sut1.Count);
        Assert.Collection(sut2,
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(true, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i),
            i => Assert.Equal(false, i));
    }

    [Fact]
    public void Next_ShouldPeekForwardUntilDefaultWithinTheList()
    {
        var sut1 = Generate.RangeOf<int?>(1024, i => i - 1).ToList();
        var sut2 = new List<int?>();

        for (var i = 1018; i < 1028; i++)
        {
            sut2.Add(sut1.Next(i));
        }

        Assert.Equal(1024, sut1.Count);
        Assert.Collection(sut2,
            i => Assert.Equal(1018, i),
            i => Assert.Equal(1019, i),
            i => Assert.Equal(1020, i),
            i => Assert.Equal(1021, i),
            i => Assert.Equal(1022, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i));
    }

    [Fact]
    public void Previous_ShouldPeekBackwardUntilDefaultWithinTheList()
    {
        var sut1 = Generate.RangeOf<int?>(1024, i => i - 1).ToList();
        var sut2 = new List<int?>();

        for (var i = 1020; i < 1030; i++)
        {
            sut2.Add(sut1.Previous(i));
        }

        Assert.Equal(1024, sut1.Count);
        Assert.Collection(sut2,
            i => Assert.Equal(1018, i),
            i => Assert.Equal(1019, i),
            i => Assert.Equal(1020, i),
            i => Assert.Equal(1021, i),
            i => Assert.Equal(1022, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i),
            i => Assert.Equal(null, i));
    }
    [Fact]
    public void HasIndex_ShouldThrowArgumentNullException_WhenListIsNull()
    {
        IList<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.HasIndex(0));
    }

    [Fact]
    public void Next_ShouldThrowArgumentNullException_WhenListIsNull()
    {
        IList<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.Next(0));
    }

    [Fact]
    public void Next_ShouldThrowArgumentOutOfRangeException_WhenIndexIsNegative()
    {
        var sut = new List<int> { 1, 2, 3 };

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Next(-1));
    }

    [Fact]
    public void Previous_ShouldThrowArgumentNullException_WhenListIsNull()
    {
        IList<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.Previous(0));
    }

    [Fact]
    public void Previous_ShouldThrowArgumentOutOfRangeException_WhenIndexIsNegative()
    {
        var sut = new List<int> { 1, 2, 3 };

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Previous(-1));
    }

    [Fact]
    public void TryAdd_ShouldAddItem_WhenMissing()
    {
        var sut = new List<int> { 1, 2, 3 };

        var result = sut.TryAdd(4);

        Assert.True(result);
        Assert.Equal(new[] { 1, 2, 3, 4 }, sut);
    }

    [Fact]
    public void TryAdd_ShouldReturnFalse_WhenItemAlreadyExists()
    {
        var sut = new List<int> { 1, 2, 3 };

        var result = sut.TryAdd(3);

        Assert.False(result);
        Assert.Equal(new[] { 1, 2, 3 }, sut);
    }

    [Fact]
    public void TryAdd_ShouldThrowArgumentNullException_WhenListIsNull()
    {
        IList<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.TryAdd(1));
    }
}
