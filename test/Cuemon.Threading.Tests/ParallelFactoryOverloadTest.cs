using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading
{
    [Trait("Category", "Threading")]
    public class ParallelFactoryOverloadTest : Test
    {
        public ParallelFactoryOverloadTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void For_ShouldSupportAllOverloads()
        {
            var actual = new ConcurrentBag<string>();
            var expected = new List<string>();

            ParallelFactory.For(0, 3, i => actual.Add($"i0:{i}"), ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i0:{i}"));

            ParallelFactory.For(0, 3, (i, a) => actual.Add($"i1:{a}:{i}"), "a", ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i1:a:{i}"));

            ParallelFactory.For(0, 3, (i, a, b) => actual.Add($"i2:{a}:{b}:{i}"), "a", "b", ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i2:a:b:{i}"));

            ParallelFactory.For(0, 3, (i, a, b, c) => actual.Add($"i3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i3:a:b:c:{i}"));

            ParallelFactory.For(0, 3, (i, a, b, c, d) => actual.Add($"i4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i4:a:b:c:d:{i}"));

            ParallelFactory.For(0, 3, (i, a, b, c, d, e) => actual.Add($"i5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureSync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i5:a:b:c:d:e:{i}"));

            ParallelFactory.For(0L, 3L, i => actual.Add($"l0:{i}"), ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l0:{i}"));

            ParallelFactory.For(0L, 3L, (i, a) => actual.Add($"l1:{a}:{i}"), "a", ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l1:a:{i}"));

            ParallelFactory.For(0L, 3L, (i, a, b) => actual.Add($"l2:{a}:{b}:{i}"), "a", "b", ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l2:a:b:{i}"));

            ParallelFactory.For(0L, 3L, (i, a, b, c) => actual.Add($"l3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l3:a:b:c:{i}"));

            ParallelFactory.For(0L, 3L, (i, a, b, c, d) => actual.Add($"l4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l4:a:b:c:d:{i}"));

            ParallelFactory.For(0L, 3L, (i, a, b, c, d, e) => actual.Add($"l5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureSync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l5:a:b:c:d:e:{i}"));

            AssertEquivalent(expected, actual);
        }

        [Fact]
        public async Task ForAsync_ShouldSupportAllOverloads()
        {
            var actual = new ConcurrentBag<string>();
            var expected = new List<string>();

            await ParallelFactory.ForAsync(0, 3, (i, ct) =>
            {
                actual.Add($"i0:{i}");
                return Task.CompletedTask;
            }, ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i0:{i}"));

            await ParallelFactory.ForAsync(0, 3, (i, a, ct) =>
            {
                actual.Add($"i1:{a}:{i}");
                return Task.CompletedTask;
            }, "a", ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i1:a:{i}"));

            await ParallelFactory.ForAsync(0, 3, (i, a, b, ct) =>
            {
                actual.Add($"i2:{a}:{b}:{i}");
                return Task.CompletedTask;
            }, "a", "b", ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i2:a:b:{i}"));

            await ParallelFactory.ForAsync(0, 3, (i, a, b, c, ct) =>
            {
                actual.Add($"i3:{a}:{b}:{c}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i3:a:b:c:{i}"));

            await ParallelFactory.ForAsync(0, 3, (i, a, b, c, d, ct) =>
            {
                actual.Add($"i4:{a}:{b}:{c}:{d}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i4:a:b:c:d:{i}"));

            await ParallelFactory.ForAsync(0, 3, (i, a, b, c, d, e, ct) =>
            {
                actual.Add($"i5:{a}:{b}:{c}:{d}:{e}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", "e", ConfigureAsync());
            expected.AddRange(Enumerable.Range(0, 3).Select(i => $"i5:a:b:c:d:e:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, ct) =>
            {
                actual.Add($"l0:{i}");
                return Task.CompletedTask;
            }, ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l0:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, a, ct) =>
            {
                actual.Add($"l1:{a}:{i}");
                return Task.CompletedTask;
            }, "a", ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l1:a:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, a, b, ct) =>
            {
                actual.Add($"l2:{a}:{b}:{i}");
                return Task.CompletedTask;
            }, "a", "b", ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l2:a:b:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, a, b, c, ct) =>
            {
                actual.Add($"l3:{a}:{b}:{c}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l3:a:b:c:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, a, b, c, d, ct) =>
            {
                actual.Add($"l4:{a}:{b}:{c}:{d}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l4:a:b:c:d:{i}"));

            await ParallelFactory.ForAsync(0L, 3L, (i, a, b, c, d, e, ct) =>
            {
                actual.Add($"l5:{a}:{b}:{c}:{d}:{e}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", "e", ConfigureAsync());
            expected.AddRange(new long[] { 0, 1, 2 }.Select(i => $"l5:a:b:c:d:e:{i}"));

            AssertEquivalent(expected, actual);
        }

        [Fact]
        public void ForResult_ShouldSupportAllOverloads()
        {
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i0:{i}"), ParallelFactory.ForResult(0, 3, i => $"i0:{i}", ConfigureSync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i1:a:{i}"), ParallelFactory.ForResult(0, 3, (i, a) => $"i1:{a}:{i}", "a", ConfigureSync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i2:a:b:{i}"), ParallelFactory.ForResult(0, 3, (i, a, b) => $"i2:{a}:{b}:{i}", "a", "b", ConfigureSync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i3:a:b:c:{i}"), ParallelFactory.ForResult(0, 3, (i, a, b, c) => $"i3:{a}:{b}:{c}:{i}", "a", "b", "c", ConfigureSync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i4:a:b:c:d:{i}"), ParallelFactory.ForResult(0, 3, (i, a, b, c, d) => $"i4:{a}:{b}:{c}:{d}:{i}", "a", "b", "c", "d", ConfigureSync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i5:a:b:c:d:e:{i}"), ParallelFactory.ForResult(0, 3, (i, a, b, c, d, e) => $"i5:{a}:{b}:{c}:{d}:{e}:{i}", "a", "b", "c", "d", "e", ConfigureSync()));

            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l0:{i}"), ParallelFactory.ForResult(0L, 3L, i => $"l0:{i}", ConfigureSync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l1:a:{i}"), ParallelFactory.ForResult(0L, 3L, (i, a) => $"l1:{a}:{i}", "a", ConfigureSync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l2:a:b:{i}"), ParallelFactory.ForResult(0L, 3L, (i, a, b) => $"l2:{a}:{b}:{i}", "a", "b", ConfigureSync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l3:a:b:c:{i}"), ParallelFactory.ForResult(0L, 3L, (i, a, b, c) => $"l3:{a}:{b}:{c}:{i}", "a", "b", "c", ConfigureSync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l4:a:b:c:d:{i}"), ParallelFactory.ForResult(0L, 3L, (i, a, b, c, d) => $"l4:{a}:{b}:{c}:{d}:{i}", "a", "b", "c", "d", ConfigureSync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l5:a:b:c:d:e:{i}"), ParallelFactory.ForResult(0L, 3L, (i, a, b, c, d, e) => $"l5:{a}:{b}:{c}:{d}:{e}:{i}", "a", "b", "c", "d", "e", ConfigureSync()));
        }

        [Fact]
        public async Task ForResultAsync_ShouldSupportAllOverloads()
        {
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i0:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, ct) => Task.FromResult($"i0:{i}"), ConfigureAsync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i1:a:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, a, ct) => Task.FromResult($"i1:{a}:{i}"), "a", ConfigureAsync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i2:a:b:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, a, b, ct) => Task.FromResult($"i2:{a}:{b}:{i}"), "a", "b", ConfigureAsync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i3:a:b:c:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, a, b, c, ct) => Task.FromResult($"i3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureAsync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i4:a:b:c:d:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, a, b, c, d, ct) => Task.FromResult($"i4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureAsync()));
            Assert.Equal(Enumerable.Range(0, 3).Select(i => $"i5:a:b:c:d:e:{i}"), await ParallelFactory.ForResultAsync(0, 3, (i, a, b, c, d, e, ct) => Task.FromResult($"i5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureAsync()));

            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l0:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, ct) => Task.FromResult($"l0:{i}"), ConfigureAsync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l1:a:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, a, ct) => Task.FromResult($"l1:{a}:{i}"), "a", ConfigureAsync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l2:a:b:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, a, b, ct) => Task.FromResult($"l2:{a}:{b}:{i}"), "a", "b", ConfigureAsync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l3:a:b:c:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, a, b, c, ct) => Task.FromResult($"l3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureAsync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l4:a:b:c:d:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, a, b, c, d, ct) => Task.FromResult($"l4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureAsync()));
            Assert.Equal(new long[] { 0, 1, 2 }.Select(i => $"l5:a:b:c:d:e:{i}"), await ParallelFactory.ForResultAsync(0L, 3L, (i, a, b, c, d, e, ct) => Task.FromResult($"l5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureAsync()));
        }

        [Fact]
        public void ForEach_ShouldSupportAllOverloads()
        {
            var source = new[] { 0, 1, 2 };
            var actual = new ConcurrentBag<string>();
            var expected = new List<string>();

            ParallelFactory.ForEach(source, i => actual.Add($"s0:{i}"), ConfigureSync());
            expected.AddRange(source.Select(i => $"s0:{i}"));

            ParallelFactory.ForEach(source, (i, a) => actual.Add($"s1:{a}:{i}"), "a", ConfigureSync());
            expected.AddRange(source.Select(i => $"s1:a:{i}"));

            ParallelFactory.ForEach(source, (i, a, b) => actual.Add($"s2:{a}:{b}:{i}"), "a", "b", ConfigureSync());
            expected.AddRange(source.Select(i => $"s2:a:b:{i}"));

            ParallelFactory.ForEach(source, (i, a, b, c) => actual.Add($"s3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureSync());
            expected.AddRange(source.Select(i => $"s3:a:b:c:{i}"));

            ParallelFactory.ForEach(source, (i, a, b, c, d) => actual.Add($"s4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureSync());
            expected.AddRange(source.Select(i => $"s4:a:b:c:d:{i}"));

            ParallelFactory.ForEach(source, (i, a, b, c, d, e) => actual.Add($"s5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureSync());
            expected.AddRange(source.Select(i => $"s5:a:b:c:d:e:{i}"));

            AssertEquivalent(expected, actual);
        }

        [Fact]
        public async Task ForEachAsync_ShouldSupportAllOverloads()
        {
            var source = new[] { 0, 1, 2 };
            var actual = new ConcurrentBag<string>();
            var expected = new List<string>();

            await ParallelFactory.ForEachAsync(source, (i, ct) =>
            {
                actual.Add($"s0:{i}");
                return Task.CompletedTask;
            }, ConfigureAsync());
            expected.AddRange(source.Select(i => $"s0:{i}"));

            await ParallelFactory.ForEachAsync(source, (i, a, ct) =>
            {
                actual.Add($"s1:{a}:{i}");
                return Task.CompletedTask;
            }, "a", ConfigureAsync());
            expected.AddRange(source.Select(i => $"s1:a:{i}"));

            await ParallelFactory.ForEachAsync(source, (i, a, b, ct) =>
            {
                actual.Add($"s2:{a}:{b}:{i}");
                return Task.CompletedTask;
            }, "a", "b", ConfigureAsync());
            expected.AddRange(source.Select(i => $"s2:a:b:{i}"));

            await ParallelFactory.ForEachAsync(source, (i, a, b, c, ct) =>
            {
                actual.Add($"s3:{a}:{b}:{c}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", ConfigureAsync());
            expected.AddRange(source.Select(i => $"s3:a:b:c:{i}"));

            await ParallelFactory.ForEachAsync(source, (i, a, b, c, d, ct) =>
            {
                actual.Add($"s4:{a}:{b}:{c}:{d}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", ConfigureAsync());
            expected.AddRange(source.Select(i => $"s4:a:b:c:d:{i}"));

            await ParallelFactory.ForEachAsync(source, (i, a, b, c, d, e, ct) =>
            {
                actual.Add($"s5:{a}:{b}:{c}:{d}:{e}:{i}");
                return Task.CompletedTask;
            }, "a", "b", "c", "d", "e", ConfigureAsync());
            expected.AddRange(source.Select(i => $"s5:a:b:c:d:e:{i}"));

            AssertEquivalent(expected, actual);
        }

        [Fact]
        public void ForEachResult_ShouldSupportAllOverloads()
        {
            var source = new[] { 0, 1, 2 };

            Assert.Equal(source.Select(i => $"s0:{i}"), ParallelFactory.ForEachResult(source, i => $"s0:{i}", ConfigureSync()));
            Assert.Equal(source.Select(i => $"s1:a:{i}"), ParallelFactory.ForEachResult(source, (i, a) => $"s1:{a}:{i}", "a", ConfigureSync()));
            Assert.Equal(source.Select(i => $"s2:a:b:{i}"), ParallelFactory.ForEachResult(source, (i, a, b) => $"s2:{a}:{b}:{i}", "a", "b", ConfigureSync()));
            Assert.Equal(source.Select(i => $"s3:a:b:c:{i}"), ParallelFactory.ForEachResult(source, (i, a, b, c) => $"s3:{a}:{b}:{c}:{i}", "a", "b", "c", ConfigureSync()));
            Assert.Equal(source.Select(i => $"s4:a:b:c:d:{i}"), ParallelFactory.ForEachResult(source, (i, a, b, c, d) => $"s4:{a}:{b}:{c}:{d}:{i}", "a", "b", "c", "d", ConfigureSync()));
            Assert.Equal(source.Select(i => $"s5:a:b:c:d:e:{i}"), ParallelFactory.ForEachResult(source, (i, a, b, c, d, e) => $"s5:{a}:{b}:{c}:{d}:{e}:{i}", "a", "b", "c", "d", "e", ConfigureSync()));
        }

        [Fact]
        public async Task ForEachResultAsync_ShouldSupportAllOverloads()
        {
            var source = new[] { 0, 1, 2 };

            Assert.Equal(source.Select(i => $"s0:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, ct) => Task.FromResult($"s0:{i}"), ConfigureAsync()));
            Assert.Equal(source.Select(i => $"s1:a:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, a, ct) => Task.FromResult($"s1:{a}:{i}"), "a", ConfigureAsync()));
            Assert.Equal(source.Select(i => $"s2:a:b:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, a, b, ct) => Task.FromResult($"s2:{a}:{b}:{i}"), "a", "b", ConfigureAsync()));
            Assert.Equal(source.Select(i => $"s3:a:b:c:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, a, b, c, ct) => Task.FromResult($"s3:{a}:{b}:{c}:{i}"), "a", "b", "c", ConfigureAsync()));
            Assert.Equal(source.Select(i => $"s4:a:b:c:d:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, a, b, c, d, ct) => Task.FromResult($"s4:{a}:{b}:{c}:{d}:{i}"), "a", "b", "c", "d", ConfigureAsync()));
            Assert.Equal(source.Select(i => $"s5:a:b:c:d:e:{i}"), await ParallelFactory.ForEachResultAsync(source, (i, a, b, c, d, e, ct) => Task.FromResult($"s5:{a}:{b}:{c}:{d}:{e}:{i}"), "a", "b", "c", "d", "e", ConfigureAsync()));
        }

        private static Action<AsyncTaskFactoryOptions> ConfigureSync()
        {
            return options =>
            {
                options.CreationOptions = TaskCreationOptions.None;
                options.PartitionSize = 2;
            };
        }

        private static Action<AsyncWorkloadOptions> ConfigureAsync()
        {
            return options => options.PartitionSize = 2;
        }

        private static void AssertEquivalent(IEnumerable<string> expected, IEnumerable<string> actual)
        {
            Assert.Equal(expected.OrderBy(item => item), actual.OrderBy(item => item));
        }
    }
}
