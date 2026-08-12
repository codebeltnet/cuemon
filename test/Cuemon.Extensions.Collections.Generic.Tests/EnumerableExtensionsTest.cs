using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Collections.Generic;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic;
public class EnumerableExtensionsTest : Test
{
    public EnumerableExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Chunk_ShouldIterateOneHundredTwentyEightItems_WhileHavingPartitions()
    {
        var sut1 = Enumerable.Range(0, 1024).Chunk();
        var sut2 = new List<int> { sut1.IteratedCount };

        while (sut1.HasPartitions)
        {
            foreach (var item in sut1)
            {

            }
            sut2.Add(sut1.IteratedCount);
        }

        Assert.False(sut1.HasPartitions);
        Assert.Equal(128, sut1.PartitionSize);
        Assert.Collection(sut2,
            i => Assert.Equal(0, i),
            i => Assert.Equal(128, i),
            i => Assert.Equal(256, i),
            i => Assert.Equal(384, i),
            i => Assert.Equal(512, i),
            i => Assert.Equal(640, i),
            i => Assert.Equal(768, i),
            i => Assert.Equal(896, i),
            i => Assert.Equal(1024, i));
    }

    [Fact]
    public void Chunk_ShouldIterateTwoHundredFiftySixItems_WhileHavingPartitions()
    {
        var sut1 = Enumerable.Range(0, 1024).Chunk(256);
        var sut2 = new List<int> { sut1.IteratedCount };

        while (sut1.HasPartitions)
        {
            foreach (var item in sut1)
            {

            }
            sut2.Add(sut1.IteratedCount);
        }

        Assert.False(sut1.HasPartitions);
        Assert.Equal(256, sut1.PartitionSize);
        Assert.Collection(sut2,
            i => Assert.Equal(0, i),
            i => Assert.Equal(256, i),
            i => Assert.Equal(512, i),
            i => Assert.Equal(768, i),
            i => Assert.Equal(1024, i));
    }

    [Fact]
    public void Shuffle_ShouldShuffleCollection()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = sut1.Shuffle().ToList();
        var sut3 = sut2.Chunk(16);

        while (sut3.HasPartitions)
        {
            TestOutput.WriteLine(DelimitedString.Create(sut3));
        }


        Assert.Equal(sut1.Count, sut2.Count);
        Assert.NotEqual(sut1, sut2);
        Assert.False(sut1.Except(sut2).Any(), "sut1.Except(sut2).Any()");
        Assert.False(sut2.Except(sut1).Any(), "sut2.Except(sut1).Any()");
    }

    [Fact]
    public void OrderAscending_ShouldOrderShuffledItems()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = sut1.Shuffle().ToList();
        var sut3 = sut2.OrderAscending().ToList();

        Assert.Equal(sut2.Count, sut3.Count);
        Assert.Equal(sut1, sut3);
        Assert.NotEqual(sut2, sut3);
        Assert.False(sut3.Except(sut2).Any(), "sut3.Except(sut2).Any()");
        Assert.False(sut2.Except(sut3).Any(), "sut2.Except(sut3).Any()");
    }

    [Fact]
    public void OrderDescending_ShouldOrderShuffledItems()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = sut1.Shuffle().ToList();
        var sut3 = sut2.OrderDescending().ToList();

        Assert.Equal(sut2.Count, sut3.Count);
        Assert.NotEqual(sut1, sut3);
        Assert.NotEqual(sut2, sut3);
        Assert.Equal(1023, sut3[0]);
        Assert.Equal(0, sut3[1023]);
        Assert.Equal(sut1.OrderDescending(), sut3);
        Assert.False(sut3.Except(sut2).Any(), "sut3.Except(sut2).Any()");
        Assert.False(sut2.Except(sut3).Any(), "sut2.Except(sut3).Any()");
    }

    [Fact]
    public void RandomOrDefault_ShouldRandomlyPickNumberFromList()
    {
        var sut1 = Enumerable.Range(0, 1024).ToList();
        var sut2 = new List<int>();

        for (var i = 0; i < sut1.Count; i++)
        {
            sut2.Add(sut1.RandomOrDefault());
        }

        TestOutput.WriteLine(DelimitedString.Create(sut2));

        Assert.Equal(sut1.Count, sut2.Count);
        Assert.NotEqual(sut1, sut2);
        Assert.NotEqual(sut1, sut2.OrderAscending());
    }

    [Fact]
    public void Yield_ShouldCreateEnumerableWithOneElement()
    {
        var sut1 = 42;
        var sut2 = sut1.Yield();


        Assert.Equal(sut1, sut2.Single());
        Assert.Collection(sut2, i => Assert.Equal(i, sut1));
        Assert.IsAssignableFrom<IEnumerable<int>>(sut2);
    }

    [Fact]
    public void ToDictionary_ShouldCreateDictionaryFromEnumerableKeyValuePair()
    {
        var sut1 = new KeyValuePair<int, string>(42, "This is the way!");
        var sut2 = sut1.Yield();
        var sut3 = sut2.ToDictionary();


        Assert.Equal(sut1, sut2.Single());
        Assert.Equal(sut2.Single(), sut3.Single());
        Assert.Collection(sut3, kvp => Assert.Equal(kvp, sut1));
        Assert.IsAssignableFrom<IDictionary<int, string>>(sut3);
    }

    [Fact]
    public void ToPartitioner_ShouldIterateOneHundredTwentyEightItems_WhileHavingPartitions()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPartitioner();
        var sut2 = new List<int> { sut1.IteratedCount };

        while (sut1.HasPartitions)
        {
            foreach (var item in sut1)
            {

            }
            sut2.Add(sut1.IteratedCount);
        }

        Assert.False(sut1.HasPartitions);
        Assert.Equal(128, sut1.PartitionSize);
        Assert.Collection(sut2,
            i => Assert.Equal(0, i),
            i => Assert.Equal(128, i),
            i => Assert.Equal(256, i),
            i => Assert.Equal(384, i),
            i => Assert.Equal(512, i),
            i => Assert.Equal(640, i),
            i => Assert.Equal(768, i),
            i => Assert.Equal(896, i),
            i => Assert.Equal(1024, i));
    }

    [Fact]
    public void ToPartitioner_ShouldIterateTwoHundredFiftySixItems_WhileHavingPartitions()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPartitioner(256);
        var sut2 = new List<int> { sut1.IteratedCount };

        while (sut1.HasPartitions)
        {
            foreach (var item in sut1)
            {

            }
            sut2.Add(sut1.IteratedCount);
        }

        Assert.False(sut1.HasPartitions);
        Assert.Equal(256, sut1.PartitionSize);
        Assert.Collection(sut2,
            i => Assert.Equal(0, i),
            i => Assert.Equal(256, i),
            i => Assert.Equal(512, i),
            i => Assert.Equal(768, i),
            i => Assert.Equal(1024, i));
    }

    [Fact]
    public void ToPagination_ShouldProvideSubsetOfSequenceWithPageSizeOfTwentyFive()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPagination(() => 1024);
        var sut2 = new PaginationOptions();


        Assert.Equal(41, sut1.PageCount);
        Assert.Equal(1024, sut1.TotalElementCount);
        Assert.True(sut1.FirstPage);
        Assert.False(sut1.LastPage);
        Assert.False(sut1.HasPreviousPage);
        Assert.True(sut1.HasNextPage);
        Assert.Equal(sut2.PageSize, sut1.Count());
    }

    [Fact]
    public void ToPagination_ShouldProvideSubsetOfSequenceWithPageSizeOfTwentyFive_FromPageFortyOne()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPagination(() => 1024, o => o.PageNumber = 41);

        Assert.Equal(41, sut1.PageCount);
        Assert.Equal(1024, sut1.TotalElementCount);
        Assert.False(sut1.FirstPage);
        Assert.True(sut1.LastPage);
        Assert.True(sut1.HasPreviousPage);
        Assert.False(sut1.HasNextPage);
        Assert.Equal(24, sut1.Count());
    }

    [Fact]
    public void ToPaginationList_ShouldProvideSubsetOfSequenceWithPageSizeOfTen()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPaginationList(() => 1024, o => o.PageSize = 10);

        Assert.Equal(0, sut1[0]);
        Assert.Equal(4, sut1[4]);
        Assert.Equal(9, sut1[9]);
        Assert.Equal(10, sut1.Count);
        Assert.Equal(103, sut1.PageCount);
        Assert.Equal(1024, sut1.TotalElementCount);
        Assert.True(sut1.FirstPage);
        Assert.False(sut1.LastPage);
        Assert.False(sut1.HasPreviousPage);
        Assert.True(sut1.HasNextPage);
        Assert.Collection(sut1,
            i => Assert.Equal(0, i),
            i => Assert.Equal(1, i),
            i => Assert.Equal(2, i),
            i => Assert.Equal(3, i),
            i => Assert.Equal(4, i),
            i => Assert.Equal(5, i),
            i => Assert.Equal(6, i),
            i => Assert.Equal(7, i),
            i => Assert.Equal(8, i),
            i => Assert.Equal(9, i));
    }

    [Fact]
    public void ToPaginationList_ShouldProvideSubsetOfSequenceWithPageSizeOfTen_FromPageOneHundredThree()
    {
        var sut1 = Enumerable.Range(0, 1024).ToPaginationList(() => 1024, o =>
        {
            o.PageSize = 10;
            o.PageNumber = 103;
        });

        Assert.Equal(1020, sut1[0]);
        Assert.Equal(1023, sut1[3]);
        Assert.Equal(4, sut1.Count);
        Assert.Equal(103, sut1.PageCount);
        Assert.Equal(1024, sut1.TotalElementCount);
        Assert.False(sut1.FirstPage);
        Assert.True(sut1.LastPage);
        Assert.True(sut1.HasPreviousPage);
        Assert.False(sut1.HasNextPage);
        Assert.Collection(sut1,
            i => Assert.Equal(1020, i),
            i => Assert.Equal(1021, i),
            i => Assert.Equal(1022, i),
            i => Assert.Equal(1023, i));
    }
    [Fact]
    public void Chunk_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.Chunk());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Chunk_ShouldThrowArgumentOutOfRangeException_WhenSizeIsInvalid(int size)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(0, 8).Chunk(size));
    }

    [Fact]
    public void Shuffle_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.Shuffle().ToList());
    }

    [Fact]
    public void Shuffle_ShouldThrowArgumentNullException_WhenRandomizerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Enumerable.Range(0, 8).Shuffle(null).ToList());
    }

    [Fact]
    public void OrderAscending_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.OrderAscending().ToList());
    }

    [Fact]
    public void OrderAscending_ShouldThrowArgumentNullException_WhenComparerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Enumerable.Range(0, 8).OrderAscending(null).ToList());
    }

    [Fact]
    public void OrderDescending_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.OrderDescending().ToList());
    }

    [Fact]
    public void OrderDescending_ShouldThrowArgumentNullException_WhenComparerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Enumerable.Range(0, 8).OrderDescending(null).ToList());
    }

    [Fact]
    public void RandomOrDefault_ShouldReturnDefault_WhenSourceIsEmpty()
    {
        var sut = Array.Empty<int>();

        Assert.Equal(default, sut.RandomOrDefault());
    }

    [Fact]
    public void RandomOrDefault_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.RandomOrDefault());
    }

    [Fact]
    public void ToDictionary_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<KeyValuePair<string, int>> sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.ToDictionary());
    }

    [Fact]
    public void ToDictionary_ShouldThrowArgumentNullException_WhenComparerIsNull()
    {
        var sut = new[]
        {
            new KeyValuePair<string, int>("a", 1)
        };

        Assert.Throws<ArgumentNullException>(() => sut.ToDictionary(null));
    }
}
