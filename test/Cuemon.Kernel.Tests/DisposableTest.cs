using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class DisposableTest : Test
{
    public DisposableTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Dispose_ShouldSetDisposedAndInvokeManagedResourcesOnce()
    {
        var sut = new ManagedOnlyDisposable();

        Assert.False(sut.Disposed);

        sut.Dispose();
        sut.Dispose();

        Assert.True(sut.Disposed);
        Assert.Equal(1, sut.ManagedDisposeCount);
    }

    [Fact]
    public void DisposeCore_ShouldOnlyInvokeUnmanagedResourcesWhenDisposingIsFalse()
    {
        var sut = new TrackingDisposable();

        sut.DisposeCore(false);

        Assert.True(sut.Disposed);
        Assert.Equal(0, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    [Fact]
    public void Dispose_ShouldInvokeManagedAndUnmanagedResourcesWhenDisposingIsTrue()
    {
        var sut = new TrackingDisposable();

        sut.Dispose();

        Assert.True(sut.Disposed);
        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    [Fact]
    public void Dispose_ShouldReleaseUnmanagedResourcesAndPreserveException_WhenManagedCleanupThrows()
    {
        var exception = new InvalidOperationException("Managed cleanup failed.");
        var sut = new ThrowingCleanupDisposable(exception);

        var actual = Assert.Throws<InvalidOperationException>(() => sut.Dispose());

        Assert.Same(exception, actual);
        Assert.True(sut.Disposed);
        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);

        sut.Dispose();

        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    [Fact]
    public void Dispose_ShouldPreserveBothExceptions_WhenBothCleanupHooksThrow()
    {
        var managedException = new InvalidOperationException("Managed cleanup failed.");
        var unmanagedException = new InvalidOperationException("Unmanaged cleanup failed.");
        var sut = new ThrowingCleanupDisposable(managedException, unmanagedException);

        var actual = Assert.Throws<AggregateException>(() => sut.Dispose());

        Assert.Collection(actual.InnerExceptions,
            exception => Assert.Same(managedException, exception),
            exception => Assert.Same(unmanagedException, exception));
        Assert.True(sut.Disposed);
        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);

        sut.Dispose();

        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DisposeCore_ShouldPreserveException_WhenOnlyUnmanagedCleanupThrows(bool disposing)
    {
        var exception = new InvalidOperationException("Unmanaged cleanup failed.");
        var sut = new ThrowingCleanupDisposable(null, exception);

        Assert.Same(exception, Assert.Throws<InvalidOperationException>(() => sut.DisposeCore(disposing)));
        Assert.True(sut.Disposed);
        Assert.Equal(disposing ? 1 : 0, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);

        sut.DisposeCore(disposing);

        Assert.Equal(disposing ? 1 : 0, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    [Fact]
    public async Task Dispose_ShouldBeThreadSafeAndInvokeCallbacksOnce()
    {
        using var managedStarted = new ManualResetEventSlim();
        using var continueDisposal = new ManualResetEventSlim();
        var sut = new BlockingDisposable(managedStarted, continueDisposal);

        var first = Task.Run(() => sut.Dispose());
        Assert.True(managedStarted.Wait(TimeSpan.FromSeconds(5)));

        var second = Task.Run(() => sut.Dispose());
        continueDisposal.Set();

        await Task.WhenAll(first, second);

        Assert.True(sut.Disposed);
        Assert.Equal(1, sut.ManagedDisposeCount);
        Assert.Equal(1, sut.UnmanagedDisposeCount);
    }

    private sealed class ThrowingCleanupDisposable : TrackingDisposable
    {
        private readonly Exception _managedException;
        private readonly Exception _unmanagedException;

        public ThrowingCleanupDisposable(Exception managedException, Exception unmanagedException = null)
        {
            _managedException = managedException;
            _unmanagedException = unmanagedException;
        }

        protected override void OnDisposeManagedResources()
        {
            base.OnDisposeManagedResources();
            if (_managedException != null) { throw _managedException; }
        }

        protected override void OnDisposeUnmanagedResources()
        {
            base.OnDisposeUnmanagedResources();
            if (_unmanagedException != null) { throw _unmanagedException; }
        }
    }
}
