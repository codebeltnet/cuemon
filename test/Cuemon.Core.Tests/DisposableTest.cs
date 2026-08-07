using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Assets;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.IO;
using Cuemon.Threading;
using Xunit;

namespace Cuemon;
public class DisposableTest : Test
{
    public DisposableTest(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void SafeInvoke_ShouldAbideRuleCA2000()
    {
        var guid = Guid.NewGuid();
        var called = 0;
        var stream = Patterns.SafeInvoke(() => new MemoryStream(), ms =>
        {
            called++;
            ms.WriteByte(1);
            ms.Position = 0;
            return ms;
        });
        Assert.NotNull(stream);
        Assert.Equal(1, called);
        Assert.Equal(1, stream.Length);

        MemoryStream msRef = null;
        called = 0;
        stream = Patterns.SafeInvoke(() => new MemoryStream(), (ms, g) =>
        {
            msRef = ms;
            Assert.Equal(guid, g);
            throw new InvalidOperationException();
        }, guid, (exception, g) =>
        {
            Assert.Equal(guid, g);
            Assert.True(exception is InvalidOperationException);
        });
        Assert.Equal(0, called);
        Assert.Null(stream);
        Assert.Throws<ObjectDisposedException>(() => msRef.Length);

        stream = Patterns.SafeInvoke(() => new MemoryStream(), (ms, n1, n2, n3, n4, n5) =>
        {
            called++;
            ms.WriteAllAsync(Decorator.Enclose($"{n1}{n2}{n3}{n4}{n5}").ToByteArray()).GetAwaiter().GetResult();
            ms.Position = 0;
            return ms;
        }, 1, 2, 3, 4, 5);
        Assert.NotNull(stream);
        Assert.Equal(1, called);
        Assert.Equal(5, stream.Length);
        Assert.Equal("12345", Decorator.Enclose(stream).ToEncodedString());
    }

    [Fact]
    public async Task SafeInvokeAsync_ShouldAbideRuleCA2000()
    {
        var guid = Guid.NewGuid();
        var called = 0;
        var stream = await AsyncPatterns.SafeInvokeAsync(() => new MemoryStream(), async (ms, ct) =>
        {
            called++;
            await ms.WriteAllAsync(new byte[] { 1 }, ct);
            ms.Position = 0;
            return ms;
        });
        Assert.NotNull(stream);
        Assert.Equal(1, called);
        Assert.Equal(1, stream.Length);

        MemoryStream msRef = null;
        called = 0;
        stream = await AsyncPatterns.SafeInvokeAsync(() => new MemoryStream(), (ms, g, ct) =>
        {
            msRef = ms;
            Assert.Equal(guid, g);
            throw new InvalidOperationException();
        }, guid, (exception, g, ct) =>
        {
            Assert.Equal(guid, g);
            Assert.True(exception is InvalidOperationException);
            return Task.CompletedTask;
        }, default);
        Assert.Equal(0, called);
        Assert.Null(stream);
        Assert.Throws<ObjectDisposedException>(() => msRef.Length);

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(5));
            msRef = null;
            called = 0;
            stream = await AsyncPatterns.SafeInvokeAsync(() => new MemoryStream(), async (ms, g, ct) =>
            {
                msRef = ms;
                Assert.Equal(guid, g);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await ms.WriteAllAsync(new byte[] { 1 }, ct);
                ms.Position = 0;
                return ms;
            }, guid, (exception, g, ct) =>
            {
                Assert.Equal(guid, g);
                Assert.True(exception is TaskCanceledException);
                return Task.CompletedTask;
            }, ctsShouldFail.Token);
            Assert.Equal(0, called);
            Assert.Null(stream);
            Assert.Throws<ObjectDisposedException>(() => msRef.Length);
        });

        stream = await AsyncPatterns.SafeInvokeAsync(() => new MemoryStream(), async (ms, n1, n2, n3, n4, n5, ct) =>
        {
            called++;
            var bytes = Decorator.Enclose($"{n1}{n2}{n3}{n4}{n5}").ToByteArray();
            await ms.WriteAllAsync(bytes, ct);
            ms.Position = 0;
            return ms;
        }, 1, 2, 3, 4, 5, default);
        Assert.NotNull(stream);
        Assert.Equal(1, called);
        Assert.Equal(5, stream.Length);
        Assert.Equal("12345", Decorator.Enclose(stream).ToEncodedString());
    }

    [Fact]
    public void ManagedDisposable_VerifyThatAssetIsBeingDisposed()
    {
        ManagedDisposable mdRef = null;
        using (var md = new ManagedDisposable())
        {
            mdRef = md;
            Assert.NotNull(md.Stream);
            Assert.Equal(0, md.Stream.Length);
            Assert.False(mdRef.Disposed);
        }
        Assert.NotNull(mdRef);
        Assert.Null(mdRef.Stream);
        Assert.True(mdRef.Disposed);
    }

    private WeakReference<UnmanagedDisposable> unmanaged = null;

    [Fact]
    public void UnmanagedDisposable_VerifyThatAssetIsBeingDisposedOnFinalize()
    {
        Action body = () =>
        {
            var o = new UnmanagedDisposable();
            Assert.NotEqual(IntPtr.Zero, o._libHandle);
            Assert.NotEqual(IntPtr.Zero, o._handle);
            unmanaged = new WeakReference<UnmanagedDisposable>(o, true);
        };

        try
        {
            body();
        }
        finally
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            Task.Delay(500).Wait(); // Add a small delay
        }

        if (unmanaged.TryGetTarget(out var ud2))
        {
            Assert.True(ud2.Disposed);
        }
    }
}
