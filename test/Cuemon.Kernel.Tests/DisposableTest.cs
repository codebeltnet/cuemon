using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
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
    }
}
