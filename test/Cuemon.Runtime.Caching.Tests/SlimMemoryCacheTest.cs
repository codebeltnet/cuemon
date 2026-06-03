using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon.Runtime.Caching.Assets;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.v3.Priority;

namespace Cuemon.Runtime.Caching
{
    [TestCaseOrderer(typeof(PriorityOrderer))]
    public class SlimMemoryCacheTest : HostTest<ManagedHostFixture>
    {
        private readonly SlimMemoryCache _cache;
        private readonly SlimMemoryCacheOptions _cacheOptions = new SlimMemoryCacheOptions();

        private const string Sliding30Namespace = "Sliding30";
        private const string Absolute30Namespace = "Absolute30";
        private const string Dependency30Namespace = "Dependency30";
        private const string Sliding60Namespace = "Sliding60";
        private const string Absolute60Namespace = "Absolute60";
        private const string Dependency60Namespace = "Dependency60";

        private const int NumberOfItemsToCache = 1000;

        public SlimMemoryCacheTest(ManagedHostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
            _cache = hostFixture.Host.Services.GetRequiredService<SlimMemoryCache>();
        }

        [Fact, Priority(-1)]
        public void Add_ShouldBeThreadSafeWhenAddingSameKeyInParallel()
        {
            var items = NumberOfItemsToCache;
            var key = "cuemon";
            Parallel.For(0, items, i =>
            {
                var nw = Guid.NewGuid();
                if (_cache.Add(key, nw, DateTime.MaxValue))
                {
                    Assert.Equal(nw, _cache[key]);
                }
                else
                {
                    Assert.NotEqual(nw, _cache[key]);
                }
            });

            Assert.Equal(1, _cache.Count());
            Assert.Equal(1, _cache.ToList().Count);

            _cache.Remove(key);

            Assert.Equal(0, _cache.Count());
            Assert.Equal(0, _cache.ToList().Count);
        }

        [Fact, Priority(0)]
        public void Add_ShouldUpdateUsingPropertyIndexer()
        {
            var key = "cuemon";
            var expectedPriorToVersionSix = "Cuemon .NET Standard";
            var expectedFromVersionSix = "Cuemon for .NET";

            _cache.Add(key, expectedPriorToVersionSix, DateTime.MaxValue);

            Assert.True(_cache.Contains(key));
            Assert.Equal(expectedPriorToVersionSix, _cache[key]);

            _cache[key] = expectedFromVersionSix;

            Assert.Equal(expectedFromVersionSix, _cache[key]);

            _cache.Remove(key);

            Assert.False(_cache.Contains(key));
        }

        [Fact, Priority(1)]
        public void Clear_ShouldRemoveAllCacheEntriesBothLogicalAndActual()
        {
            var values = Enumerable.Range(0, 10000).ToList();

            foreach (var value in values)
            {
                _cache.Add(Guid.NewGuid().ToString("N"), value, DateTime.MaxValue);
            }

            Assert.Equal(values.Count, _cache.Count());
            Assert.Equal(values.Count, _cache.ToList().Count);
            Assert.True(values.Count == _cache.Select(pair => pair.Value.CanExpire).Count(), "values.Count == _cache.Select(pair => pair.Value.CanExpire).Count()");

            _cache.RemoveAll();

            Assert.Equal(0, _cache.Count());
            Assert.Equal(0, _cache.ToList().Count);
        }

        [Fact, Priority(2)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingSlidingExpirationOfOneMinute()
        {
            var items = NumberOfItemsToCache;
            var expires = TimeSpan.FromMinutes(1);
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Sliding60Namespace));
                _cache.Add(key, Generate.RandomString(5), expires, Sliding60Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(items, _cache.Count(Sliding60Namespace));
            Assert.Equal(items, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Sliding60Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(3)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingSlidingExpirationOfThirtySecondsWithNamespaceSpecification()
        {
            var items = NumberOfItemsToCache;
            var expires = TimeSpan.FromSeconds(30);
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Sliding30Namespace));
                _cache.Add(key, Generate.RandomString(5), expires, Sliding30Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(keys.Count, _cache.Count(Sliding30Namespace));
            Assert.Equal(items * 2, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Sliding30Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(4)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingAbsoluteExpirationOfOneMinute()
        {
            var items = NumberOfItemsToCache;
            var expires = DateTime.UtcNow.AddMinutes(1);
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Absolute60Namespace));
                _cache.Add(key, Generate.RandomString(5), expires, Absolute60Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(items, _cache.Count(Absolute60Namespace));
            Assert.Equal(items * 3, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Absolute60Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(5)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingAbsoluteExpirationOfThirtySecondsWithNamespaceSpecification()
        {
            var items = NumberOfItemsToCache;
            var expires = DateTime.UtcNow.AddSeconds(30);
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Absolute30Namespace));
                _cache.Add(key, Generate.RandomString(5), expires, Absolute30Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(keys.Count, _cache.Count(Absolute30Namespace));
            Assert.Equal(items * 4, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Absolute30Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(6)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingDependencyExpirationOfOneMinute()
        {
            var items = NumberOfItemsToCache;
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromMinutes(1)));
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Dependency60Namespace));
                _cache.Add(key, Generate.RandomString(5), expires(), Dependency60Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(items, _cache.Count(Dependency60Namespace));
            Assert.Equal(items * 5, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Dependency60Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(7)]
        public void Add_ShouldHandleLargeLoadWithoutCollisionUsingDependencyExpirationOfThirtySecondsWithNamespaceSpecification()
        {
            var items = NumberOfItemsToCache;
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(30)));
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the SlimMemoryCache
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, Dependency30Namespace));
                _cache.Add(key, Generate.RandomString(5), expires(), Dependency30Namespace);
            });

            Assert.Equal(0, _cache.Count());
            Assert.Equal(items, _cache.Count(Dependency30Namespace));
            Assert.Equal(items * 6, _cache.ToList().Count);
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == Dependency30Namespace).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation
        }

        [Fact, Priority(8)]
        public void Add_VerifyBothLogicalAndActualCacheRemovalUponExpirationForThirtySecondsNamespaceSpecification()
        {
            Thread.Sleep(TimeSpan.FromSeconds(30));

            Assert.Equal(0, _cache.Count(Dependency30Namespace));
            Assert.Equal(0, _cache.Count(Sliding30Namespace));
            Assert.Equal(0, _cache.Count(Absolute30Namespace));

            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Dependency30Namespace).ToList().Count);
            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Sliding30Namespace).ToList().Count);
            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Absolute30Namespace).ToList().Count);

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Dependency30Namespace).ToList().Count);
            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Sliding30Namespace).ToList().Count);
            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Absolute30Namespace).ToList().Count);
        }

        [Fact, Priority(9)]
        public void Add_VerifyBothLogicalAndActualCacheRemovalUponExpirationForSixtySecondsNamespaceSpecification()
        {
            Thread.Sleep(TimeSpan.FromSeconds(20));

            Assert.Equal(0, _cache.Count(Dependency60Namespace));
            Assert.Equal(0, _cache.Count(Sliding60Namespace));
            Assert.Equal(0, _cache.Count(Absolute60Namespace));

            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Dependency60Namespace).ToList().Count);
            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Sliding60Namespace).ToList().Count);
            Assert.Equal(NumberOfItemsToCache, _cache.Where(pair => pair.Value.Namespace == Absolute60Namespace).ToList().Count);

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Dependency60Namespace).ToList().Count);
            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Sliding60Namespace).ToList().Count);
            Assert.Equal(0, _cache.Where(pair => pair.Value.Namespace == Absolute60Namespace).ToList().Count);
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

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Action<SlimMemoryCacheOptions>>(o =>
            {
                o.FirstSweep = TimeSpan.FromSeconds(35);
                o.SucceedingSweep = TimeSpan.FromSeconds(5);
            });
            services.AddSingleton<SlimMemoryCache>();
        }
    }
}