using System;
using System.Threading;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Runtime.Caching
{
    public class SlimMemoryCacheCoverageTest : Test
    {
        public SlimMemoryCacheCoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MissingMembers_ShouldReturnFalseOrNull_WhenEntryDoesNotExist()
        {
            var sut = new SlimMemoryCache();

            var result = sut.TryGet("missing", out var value);

            Assert.False(sut.Contains("missing"));
            Assert.Null(sut.Get("missing"));
            Assert.Null(sut.GetCacheEntry("missing"));
            Assert.Null(sut.Remove("missing"));
            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void Set_ShouldInsertAndUpdateEntry_WhenCalled()
        {
            var sut = new SlimMemoryCache();
            var invalidation = new CacheInvalidation(DateTime.UtcNow.AddMinutes(1));

            sut.Set("key", "value", invalidation);
            var beforeUpdate = sut.GetCacheEntry("key");
            var accessed = beforeUpdate.Accessed;

            Thread.Sleep(20);
            sut.Set("key", "updated", invalidation);
            var afterUpdate = sut.GetCacheEntry("key");

            Assert.NotNull(beforeUpdate);
            Assert.NotNull(afterUpdate);
            Assert.Equal("updated", sut.Get("key"));
            Assert.Equal(1, sut.Count());
            Assert.True(afterUpdate.Accessed >= accessed);
        }

        [Fact]
        public void RemoveAll_ShouldRemoveEntriesWithinSpecifiedNamespace()
        {
            var sut = new SlimMemoryCache();
            var invalidation = new CacheInvalidation(DateTime.UtcNow.AddMinutes(1));

            sut.Set("key1", "value1", invalidation, "ns1");
            sut.Set("key2", "value2", invalidation, "ns1");
            sut.Set("key3", "value3", invalidation, "ns2");

            sut.RemoveAll("ns1");

            Assert.Equal(0, sut.Count("ns1"));
            Assert.Equal(1, sut.Count("ns2"));
            Assert.False(sut.Contains("key1", "ns1"));
            Assert.False(sut.Contains("key2", "ns1"));
            Assert.True(sut.Contains("key3", "ns2"));
        }
    }
}
