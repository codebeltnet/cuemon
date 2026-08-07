using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading;
[Trait("Category", "Threading")]
public class AsyncPatternsTest : Test
{
    public AsyncPatternsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Use_ShouldReturnSingleton()
    {
        Assert.Same(AsyncPatterns.Use, AsyncPatterns.Use);
    }

    [Fact]
    public async Task SafeInvokeAsync_ShouldSupportAllOverloads_WhenTesterSucceeds()
    {
        var actual = new List<string>();
        var cts = new CancellationTokenSource();

        var result0 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("0"), async (probe, ct) =>
        {
            await Task.Yield();
            actual.Add($"0:{probe.Value}:{ct.CanBeCanceled}");
            return probe;
        }, ct: cts.Token);

        var result1 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("1"), async (probe, arg, ct) =>
        {
            await Task.Yield();
            actual.Add($"1:{probe.Value}:{arg}:{ct.CanBeCanceled}");
            return probe;
        }, "a", ct: cts.Token);

        var result2 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("2"), async (probe, arg1, arg2, ct) =>
        {
            await Task.Yield();
            actual.Add($"2:{probe.Value}:{arg1}:{arg2}:{ct.CanBeCanceled}");
            return probe;
        }, "a", "b", ct: cts.Token);

        var result3 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("3"), async (probe, arg1, arg2, arg3, ct) =>
        {
            await Task.Yield();
            actual.Add($"3:{probe.Value}:{arg1}:{arg2}:{arg3}:{ct.CanBeCanceled}");
            return probe;
        }, "a", "b", "c", ct: cts.Token);

        var result4 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("4"), async (probe, arg1, arg2, arg3, arg4, ct) =>
        {
            await Task.Yield();
            actual.Add($"4:{probe.Value}:{arg1}:{arg2}:{arg3}:{arg4}:{ct.CanBeCanceled}");
            return probe;
        }, "a", "b", "c", "d", ct: cts.Token);

        var result5 = await AsyncPatterns.SafeInvokeAsync(() => new DisposableProbe("5"), async (probe, arg1, arg2, arg3, arg4, arg5, ct) =>
        {
            await Task.Yield();
            actual.Add($"5:{probe.Value}:{arg1}:{arg2}:{arg3}:{arg4}:{arg5}:{ct.CanBeCanceled}");
            return probe;
        }, "a", "b", "c", "d", "e", ct: cts.Token);

        Assert.Equal(new[]
        {
            "0:0:True",
            "1:1:a:True",
            "2:2:a:b:True",
            "3:3:a:b:c:True",
            "4:4:a:b:c:d:True",
            "5:5:a:b:c:d:e:True"
        }, actual);

        Assert.False(result0.IsDisposed);
        Assert.False(result1.IsDisposed);
        Assert.False(result2.IsDisposed);
        Assert.False(result3.IsDisposed);
        Assert.False(result4.IsDisposed);
        Assert.False(result5.IsDisposed);

        result0.Dispose();
        result1.Dispose();
        result2.Dispose();
        result3.Dispose();
        result4.Dispose();
        result5.Dispose();
    }

    [Fact]
    public async Task SafeInvokeAsync_ShouldInvokeCatcherAndDisposeInitializer_WhenTesterThrows()
    {
        var actual = new List<string>();
        DisposableProbe initializer0 = null;
        DisposableProbe initializer1 = null;
        DisposableProbe initializer2 = null;
        DisposableProbe initializer3 = null;
        DisposableProbe initializer4 = null;
        DisposableProbe initializer5 = null;

        var result0 = await AsyncPatterns.SafeInvokeAsync(() => initializer0 = new DisposableProbe("0"), async (probe, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("0");
        }, async (ex, ct) =>
        {
            await Task.Yield();
            actual.Add($"0:{ex.Message}");
        });

        var result1 = await AsyncPatterns.SafeInvokeAsync(() => initializer1 = new DisposableProbe("1"), async (probe, arg, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("1");
        }, "a", async (ex, arg, ct) =>
        {
            await Task.Yield();
            actual.Add($"1:{arg}:{ex.Message}");
        });

        var result2 = await AsyncPatterns.SafeInvokeAsync(() => initializer2 = new DisposableProbe("2"), async (probe, arg1, arg2, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("2");
        }, "a", "b", async (ex, arg1, arg2, ct) =>
        {
            await Task.Yield();
            actual.Add($"2:{arg1}:{arg2}:{ex.Message}");
        });

        var result3 = await AsyncPatterns.SafeInvokeAsync(() => initializer3 = new DisposableProbe("3"), async (probe, arg1, arg2, arg3, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("3");
        }, "a", "b", "c", async (ex, arg1, arg2, arg3, ct) =>
        {
            await Task.Yield();
            actual.Add($"3:{arg1}:{arg2}:{arg3}:{ex.Message}");
        });

        var result4 = await AsyncPatterns.SafeInvokeAsync(() => initializer4 = new DisposableProbe("4"), async (probe, arg1, arg2, arg3, arg4, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("4");
        }, "a", "b", "c", "d", async (ex, arg1, arg2, arg3, arg4, ct) =>
        {
            await Task.Yield();
            actual.Add($"4:{arg1}:{arg2}:{arg3}:{arg4}:{ex.Message}");
        });

        var result5 = await AsyncPatterns.SafeInvokeAsync(() => initializer5 = new DisposableProbe("5"), async (probe, arg1, arg2, arg3, arg4, arg5, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("5");
        }, "a", "b", "c", "d", "e", async (ex, arg1, arg2, arg3, arg4, arg5, ct) =>
        {
            await Task.Yield();
            actual.Add($"5:{arg1}:{arg2}:{arg3}:{arg4}:{arg5}:{ex.Message}");
        });

        Assert.Null(result0);
        Assert.Null(result1);
        Assert.Null(result2);
        Assert.Null(result3);
        Assert.Null(result4);
        Assert.Null(result5);

        Assert.Equal(new[]
        {
            "0:0",
            "1:a:1",
            "2:a:b:2",
            "3:a:b:c:3",
            "4:a:b:c:d:4",
            "5:a:b:c:d:e:5"
        }, actual);

        Assert.True(initializer0.IsDisposed);
        Assert.True(initializer1.IsDisposed);
        Assert.True(initializer2.IsDisposed);
        Assert.True(initializer3.IsDisposed);
        Assert.True(initializer4.IsDisposed);
        Assert.True(initializer5.IsDisposed);
    }

    [Fact]
    public async Task SafeInvokeAsync_ShouldRethrowAndDisposeInitializer_WhenNoCatcherIsProvided()
    {
        DisposableProbe initializer = null;

        await Assert.ThrowsAsync<InvalidOperationException>(() => AsyncPatterns.SafeInvokeAsync(() => initializer = new DisposableProbe("boom"), async (probe, ct) =>
        {
            await Task.Yield();
            throw new InvalidOperationException("boom");
        }));

        Assert.True(initializer.IsDisposed);
    }

    private sealed class DisposableProbe : IDisposable
    {
        public DisposableProbe(string value)
        {
            Value = value;
        }

        public bool IsDisposed { get; private set; }

        public string Value { get; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
