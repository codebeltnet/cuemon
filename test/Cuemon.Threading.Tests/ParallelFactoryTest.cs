using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading;
[Trait("Category", "Threading")]
public class ParallelFactoryTest : Test
{
    private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    private readonly TimeSpan _maxAllowedTestTime = TimeSpan.FromMinutes(1);
    private readonly TimeSpan _longRunningTaskWaitTime = IsLinux ? TimeSpan.FromMilliseconds(1) : TimeSpan.FromMilliseconds(10);

    public ParallelFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void For_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var expected = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.For(0, count, i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void For_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            ParallelFactory.For(0, count, i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void For_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = sbyte.MaxValue;
        var expected = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.For(0, count, i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
        }, o => o.CancellationToken = cts.Token);

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void For_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var expected = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.For(0, count, i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForResult_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForResult(0, count, i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForResult_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            ParallelFactory.ForResult(0, count, i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void ForResult_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = sbyte.MaxValue;
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForResult(0, count, i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
            return i;
        }, o => o.CancellationToken = cts.Token);

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForResult(0, count, i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEach_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.ForEach(ic, i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEach_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            ParallelFactory.ForEach(ic, i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void ForEach_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = sbyte.MaxValue;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.ForEach(ic, i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
        }, o => o.CancellationToken = cts.Token);

        Assert.Equal(count, cb.Count);
        Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEach_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        ParallelFactory.ForEach(ic, i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(ic.SequenceEqual(cb.OrderBy(i => i)), "ic.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEachResult_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForEachResult(ic, i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEachResult_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            ParallelFactory.ForEachResult(ic, i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void ForEachResult_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = sbyte.MaxValue;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForEachResult(ic, i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
            return i;
        }, o => o.CancellationToken = cts.Token);

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void ForEachResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var ic = Generate.RangeOf(count, i => i);
        var cb = new ConcurrentBag<int>();

        var result = ParallelFactory.ForEachResult(ic, i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void While_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        AdvancedParallelFactory.While(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void While_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            AdvancedParallelFactory.While(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void While_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var count = sbyte.MaxValue;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        AdvancedParallelFactory.While(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
        });

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void While_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        AdvancedParallelFactory.While(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(expected.SequenceEqual(cb.OrderBy(i => i)), "expected.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void WhileResult_ShouldRunConcurrent()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 1000;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        var result = AdvancedParallelFactory.WhileResult(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(50);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.CreationOptions = TaskCreationOptions.None;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void WhileResult_ShouldRunConcurrent_IgniteCancellation()
    {
        var count = 1000;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();
        var cts = new CancellationTokenSource();
        var x = 0;
        var ae = Assert.Throws<AggregateException>(() =>
        {
            AdvancedParallelFactory.WhileResult(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
            {
                Interlocked.Increment(ref x);
                if (i > 450) { cts.Cancel(); }
                Thread.Sleep(Generate.RandomNumber(25, 75));
                cb.Add(i);
                return i;
            }, o =>
            {
                o.CancellationToken = cts.Token;
                o.CreationOptions = TaskCreationOptions.None;
            });
        });

        Assert.IsAssignableFrom<OperationCanceledException>(ae.InnerExceptions.FirstOrDefault(ex => ex.GetType().IsAssignableFrom(typeof(TaskCanceledException))));

        TestOutput.WriteLine(x.ToString());
        TestOutput.WriteLine($"Threads processed: {cb.Count}.");

        Assert.InRange(cb.Count, 200, 500); // most threads should have executed before cancellation
    }

    [Fact]
    public void WhileResult_ShouldRunConcurrent_LongRunning_SystemPartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = sbyte.MaxValue;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        var result = AdvancedParallelFactory.WhileResult(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(_longRunningTaskWaitTime);
            cb.Add(i);
            return i;
        }, o => o.CancellationToken = cts.Token);

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    [Fact]
    public void WhileResult_ShouldRunConcurrent_LongRunning_ExtremePartition()
    {
        var cts = new CancellationTokenSource(_maxAllowedTestTime);
        var count = 8192;
        var expected = Generate.RangeOf(count, i => i);
        var ic = new Queue<int>(expected);
        var cb = new ConcurrentBag<int>();

        var result = AdvancedParallelFactory.WhileResult(ic, () => ic.Count > 0, intProvider => intProvider.Dequeue(), i =>
        {
            Thread.Sleep(5);
            cb.Add(i);
            return i;
        }, o =>
        {
            o.CancellationToken = cts.Token;
            o.PartitionSize = MaxThreadCount;
        });

        Assert.Equal(count, cb.Count);
        Assert.True(result.SequenceEqual(cb.OrderBy(i => i)), "result.SequenceEqual(cb.OrderBy(i => i))");
    }

    private static int MaxThreadCount => IsLinux ? 1024 : 4096;
}
