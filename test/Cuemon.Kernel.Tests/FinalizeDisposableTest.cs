using System;
using System.Threading;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class FinalizeDisposableTest : Test
{
    public FinalizeDisposableTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Dispose_ShouldSetDisposedAndInvokeUnmanagedResources()
    {
        TrackingFinalizeDisposable.Reset();
        var sut = new TrackingFinalizeDisposable();

        sut.Dispose();

        Assert.True(sut.Disposed);
        Assert.Equal(1, TrackingFinalizeDisposable.UnmanagedDisposeCount);
    }

    [Fact]
    public void DisposeCore_ShouldSetDisposedAndInvokeUnmanagedResourcesWhenDisposingIsFalse()
    {
        TrackingFinalizeDisposable.Reset();
        var sut = new TrackingFinalizeDisposable();

        sut.DisposeCore(false);

        Assert.True(sut.Disposed);
        Assert.Equal(1, TrackingFinalizeDisposable.UnmanagedDisposeCount);
    }

    [Fact]
    public void Finalizer_ShouldInvokeUnmanagedResources()
    {
        TrackingFinalizeDisposable.Reset();
        CreateFinalizableInstance();

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

        Assert.True(SpinWait.SpinUntil(() => TrackingFinalizeDisposable.UnmanagedDisposeCount == 1, TimeSpan.FromSeconds(5)));
    }

    [Fact]
    public void Dispose_ShouldSuppressFinalizer()
    {
        TrackingFinalizeDisposable.Reset();
        CreateDisposedInstance();

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

        Assert.Equal(1, TrackingFinalizeDisposable.UnmanagedDisposeCount);
    }

    private static void CreateFinalizableInstance()
    {
        _ = new TrackingFinalizeDisposable();
    }

    private static void CreateDisposedInstance()
    {
        var sut = new TrackingFinalizeDisposable();
        sut.Dispose();
    }
}
