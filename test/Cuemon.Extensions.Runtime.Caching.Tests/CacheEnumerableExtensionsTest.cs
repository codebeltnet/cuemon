using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Runtime.Caching.Assets;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon.Runtime;
using Cuemon.Runtime.Caching;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.Runtime.Caching
{
    public class CacheEnumerableExtensionsTest : HostTest<ManagedHostFixture>
    {
        private readonly SlimMemoryCache _cache;
        private readonly SlimMemoryCacheOptions _cacheOptions = new SlimMemoryCacheOptions();

        public CacheEnumerableExtensionsTest(ManagedHostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
            _cache = hostFixture.Host.Services.GetRequiredService<SlimMemoryCache>();
        }

        [Fact]
        public void GetOrAdd_ShouldCacheAndReturnItemInOneGoUsingSlidingExpirationOfTenSeconds()
        {
            var items = 1000;
            var expires = TimeSpan.FromSeconds(10);
            var keys = Generate.RangeOf(items, i => Guid.NewGuid().ToString("N")).ToList();
            var bag = new ConcurrentBag<long>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.ForEach(keys, key =>
            {
                bag.Add(_cacheOptions.KeyProvider(key, CacheEntry.NoScope));
                var value = Generate.RandomString(5);
                Assert.Equal(value, _cache.GetOrAdd(key, expires, () => value));
            });

            Assert.Equal(items, _cache.Count());
            Assert.True(bag.OrderBy(l => l).SequenceEqual(_cache.Where(pair => pair.Value.Namespace == CacheEntry.NoScope).Select(pair => pair.Key).OrderBy(l => l))); // insure thread safety validation

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count());
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<string>(ExpensiveRandomString);
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs());
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(17, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingOneParameterUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, string>(p1 =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(3, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingTwoParameterUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, string>((p1, p2) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(2, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingThreeParameterUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, string>((p1, p2, p3) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(5, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFourParameterUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, int, string>((p1, p2, p3, p4) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(7, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFiveParameterUsingSlidingExpirationOfTenSeconds()
        {
            var expires = TimeSpan.FromSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<byte, int, int, int, int, string>((p1, p2, p3, p4, p5) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4 + p5);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2, 4));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(11, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<string>(ExpensiveRandomString);
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs());
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(17, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingOneParameterUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, string>(p1 =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(3, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingTwoParameterUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, string>((p1, p2) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(2, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingThreeParameterUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, string>((p1, p2, p3) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(5, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFourParameterUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, int, string>((p1, p2, p3, p4) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(7, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFiveParameterUsingAbsoluteExpirationOfTenSeconds()
        {
            var expires = DateTime.UtcNow.AddSeconds(10);
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<byte, int, int, int, int, string>((p1, p2, p3, p4, p5) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4 + p5);
                });
                var rs = _cache.Memoize(expires, value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2, 4));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(11, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<string>(ExpensiveRandomString);
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs());
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(17, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingOneParameterUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, string>(p1 =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1);
                });
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(3, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingTwoParameterUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, string>((p1, p2) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2);
                });
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(2, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingThreeParameterUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, string>((p1, p2, p3) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3);
                });
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(5, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFourParameterUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<int, int, int, int, string>((p1, p2, p3, p4) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4);
                });
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(7, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void Memoize_ShouldCacheAndReturnFunctionDelegateHavingFiveParameterUsingDependencyExpirationOfTenSeconds()
        {
            var expires = new Func<CountdownDependency>(() => new CountdownDependency(TimeSpan.FromSeconds(10)));
            var timeSpans = new ConcurrentBag<TimeSpan>();
            var values = new ConcurrentBag<string>();

            // we use Parallel because we want to assure thread safety of the extension method
            Parallel.For(0, 1000, i =>
            {
                var value = new Func<byte, int, int, int, int, string>((p1, p2, p3, p4, p5) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return Generate.RandomString(p1 + p2 + p3 + p4 + p5);
                });
                var rs = _cache.Memoize(expires(), value);
                var sw = Stopwatch.StartNew();
                values.Add(rs(1, 1, 3, 2, 4));
                sw.Stop();
                timeSpans.Add(sw.Elapsed);
            });

            var s = Assert.Single(values.Distinct());
            Assert.Equal(11, s.Length);
            Assert.True(Condition.IsPrime(s.Length));

            var turtle = timeSpans.Where(ts => ts > TimeSpan.FromSeconds(1)).ToList();
            var rabbit = timeSpans.Where(ts => ts < TimeSpan.FromSeconds(1)).ToList();

            foreach (var writeLockHit in turtle)
            {
                Assert.InRange(writeLockHit, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            }

            TestOutput.WriteLine($"Suspected hit-rate of PadLock: {turtle.Count}.");
            TestOutput.WriteLine($"The rest, {rabbit.Count}, had {rabbit.Count(ts => ts < TimeSpan.FromMilliseconds(25))} in expected range (<25ms).");
            TestOutput.WriteLine(s);

            foreach (var nonLockHit in rabbit)
            {
                Assert.InRange(nonLockHit, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }

            Assert.Equal(1, _cache.Count(CacheEnumerableExtensions.MemoizationScope));

            Thread.Sleep(TimeSpan.FromSeconds(11));

            Assert.Equal(0, _cache.Count(CacheEnumerableExtensions.MemoizationScope));
        }

        [Fact]
        public void GetOrAdd_ShouldUseDependencyOverload_AndReturnCachedValue()
        {
            var cache = CreateCacheForExtensionTests();
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
            var cache = CreateCacheForExtensionTests();
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
            var cache = CreateCacheForExtensionTests();
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
            var cache = CreateCacheForExtensionTests();
            var invalidation = new CacheInvalidation(TimeSpan.FromMinutes(1));
            var exception = Assert.Throws<ArgumentNullException>(() => cache.GetOrAdd(null, "scope", invalidation, () => "value"));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenInvalidationIsNull()
        {
            var cache = CreateCacheForExtensionTests();
            var exception = Assert.Throws<ArgumentNullException>(() => CacheEnumerableExtensions.GetOrAdd<long, string>(cache, "key", "scope", (CacheInvalidation)null, () => "value"));

            Assert.Equal("invalidation", exception.ParamName);
        }

        [Fact]
        public void GetOrAdd_ShouldThrowArgumentNullException_WhenValueFactoryIsNull()
        {
            var cache = CreateCacheForExtensionTests();
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
            var cache = CreateCacheForExtensionTests();
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

        private string ExpensiveRandomString()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return Generate.RandomString(17);
        }

        private static SlimMemoryCache CreateCacheForExtensionTests()
        {
            return new SlimMemoryCache(o => o.EnableCleanup = false);
        }

        private static IEnumerable<IDependency> CreateDependencies()
        {
            return new IDependency[] { new PassiveDependency(), new PassiveDependency() };
        }

        private static void AssertMemoizeEnumerableDependencies<TDelegate>(Func<SlimMemoryCache, TDelegate> factory, Func<TDelegate, string> invoke, int expectedLength)
        {
            var cache = CreateCacheForExtensionTests();
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

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SlimMemoryCache>();
        }
    }
}