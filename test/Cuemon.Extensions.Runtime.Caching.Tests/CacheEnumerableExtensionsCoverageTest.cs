using System;
using System.Collections.Generic;
using System.Text;
using Cuemon.Runtime;
using Cuemon.Runtime.Caching;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Runtime.Caching
{
    public class CacheEnumerableExtensionsCoverageTest : Test
    {
        public CacheEnumerableExtensionsCoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetOrAdd_ShouldUseDependencyOverload_AndReturnCachedValue()
        {
            var cache = CreateCache();
            var key = Generate.RandomString(16);
            var dependency = new PassiveDependency();
            var factoryCalls = 0;

            var first = cache.GetOrAdd(key, dependency, () =>
            {
                factoryCalls++;
                return "alpha";
            });
            var second = cache.GetOrAdd(key, dependency, () =>
            {
                factoryCalls++;
                return "beta";
            });

            Assert.Equal("alpha", first);
            Assert.Equal("alpha", second);
            Assert.Equal(1, factoryCalls);
            Assert.True(cache.Contains(key));
        }

        [Fact]
        public void GetOrAdd_ShouldUseDependenciesOverload_AndReturnCachedValue()
        {
            var cache = CreateCache();
            var key = Generate.RandomString(16);
            var dependencies = CreateDependencies();
            var factoryCalls = 0;

            var first = cache.GetOrAdd(key, dependencies, () =>
            {
                factoryCalls++;
                return 42;
            });
            var second = cache.GetOrAdd(key, dependencies, () =>
            {
                factoryCalls++;
                return 84;
            });

            Assert.Equal(42, first);
            Assert.Equal(42, second);
            Assert.Equal(1, factoryCalls);
            Assert.True(cache.Contains(key));
        }

        [Fact]
        public void GetOrAdd_ShouldUseInvalidationOverload_AndReturnCachedValueOnCacheHit()
        {
            var cache = CreateCache();
            var key = Generate.RandomString(16);
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var factoryCalls = 0;

            var first = cache.GetOrAdd(key, invalidation, () =>
            {
                factoryCalls++;
                return Guid.Empty;
            });
            var second = cache.GetOrAdd(key, invalidation, () =>
            {
                factoryCalls++;
                return Guid.NewGuid();
            });

            Assert.Equal(Guid.Empty, first);
            Assert.Equal(Guid.Empty, second);
            Assert.Equal(1, factoryCalls);
            Assert.True(cache.Contains(key));
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenCacheIsNull()
        {
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var exception = Assert.Throws<ArgumentNullException>(() => CacheEnumerableExtensions.GetOrAdd<long, string>(null, "key", "scope", invalidation, () => "value"));

            Assert.Equal("cache", exception.ParamName);
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenKeyIsNull()
        {
            var cache = CreateCache();
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var exception = Assert.Throws<ArgumentNullException>(() => cache.GetOrAdd(null, "scope", invalidation, () => "value"));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenInvalidationIsNull()
        {
            var cache = CreateCache();
            var exception = Assert.Throws<ArgumentNullException>(() => CacheEnumerableExtensions.GetOrAdd<long, string>(cache, "key", "scope", (CacheInvalidation)null, () => "value"));

            Assert.Equal("invalidation", exception.ParamName);
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenValueFactoryIsNull()
        {
            var cache = CreateCache();
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var exception = Assert.Throws<ArgumentNullException>(() => cache.GetOrAdd<long, string>("key", "scope", invalidation, null));

            Assert.Equal("valueFactory", exception.ParamName);
        }

        [Fact]
        public void Memoize_ShouldCacheEnumerableDependencyOverloads()
        {
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<string>(() => Generate.RandomString(5))), memoized => memoized(), 5);
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<int, string>(length => Generate.RandomString(length))), memoized => memoized(3), 3);
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<int, int, string>((a, b) => Generate.RandomString(a + b))), memoized => memoized(2, 3), 5);
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<int, int, int, string>((a, b, c) => Generate.RandomString(a + b + c))), memoized => memoized(1, 2, 4), 7);
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<int, int, int, int, string>((a, b, c, d) => Generate.RandomString(a + b + c + d))), memoized => memoized(1, 2, 3, 5), 11);
            AssertMemoizeEnumerableDependencies(cache => cache.Memoize(CreateDependencies(), new Func<int, int, int, int, int, string>((a, b, c, d, e) => Generate.RandomString(a + b + c + d + e))), memoized => memoized(1, 2, 3, 5, 2), 13);
        }

        [Fact]
        public void Memoize_ShouldUseArgumentValuesForCacheKeys_WhenArgumentsAreNullOrByteArrays()
        {
            var cache = CreateCache();
            var byteArrayCalls = 0;
            var nullCalls = 0;
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var memoizedBytes = cache.Memoize(invalidation, new Func<byte[], string>(bytes =>
            {
                byteArrayCalls++;
                return Convert.ToBase64String(bytes ?? Array.Empty<byte>());
            }));
            var memoizedNull = cache.Memoize(invalidation, new Func<string, string>(value =>
            {
                nullCalls++;
                return value ?? "missing";
            }));

            Assert.Equal("AQID", memoizedBytes(new byte[] { 1, 2, 3 }));
            Assert.Equal("AQID", memoizedBytes(new byte[] { 1, 2, 3 }));
            Assert.Equal("missing", memoizedNull(null));
            Assert.Equal("missing", memoizedNull(null));
            Assert.Equal(1, byteArrayCalls);
            Assert.Equal(1, nullCalls);
            Assert.Equal(2, cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        private static SlimMemoryCache CreateCache()
        {
            return new SlimMemoryCache(o => o.EnableCleanup = false);
        }

        private static IEnumerable<IDependency> CreateDependencies()
        {
            return new IDependency[] { new PassiveDependency(), new PassiveDependency() };
        }

        private static void AssertMemoizeEnumerableDependencies<TDelegate>(Func<SlimMemoryCache, TDelegate> factory, Func<TDelegate, string> invoke, int expectedLength)
        {
            var cache = CreateCache();
            var memoized = factory(cache);

            var first = invoke(memoized);
            var second = invoke(memoized);

            Assert.Equal(first, second);
            Assert.Equal(expectedLength, first.Length);
            Assert.Equal(1, cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        private sealed class PassiveDependency : Dependency
        {
            public PassiveDependency() : base(_ => Array.Empty<IWatcher>(), true)
            {
            }
        }
    }
}
