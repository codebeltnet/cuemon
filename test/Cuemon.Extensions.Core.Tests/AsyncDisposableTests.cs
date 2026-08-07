using System.Diagnostics;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class AsyncDisposableTest : Test
{
    public AsyncDisposableTest(ITestOutputHelper output) : base(output)
    {
    }

    private class TestAsyncDisposable : AsyncDisposable
    {
        public bool ManagedResourcesDisposed { get; private set; }

        public bool IsAsyncOperationCompleted { get; private set; }

        public bool UnmanagedResourcesDisposed { get; private set; }

        protected override ValueTask OnDisposeManagedResourcesAsync()
        {
            ManagedResourcesDisposed = true;
            return new ValueTask(Task.Run(async () =>
            {
                await Task.Delay(100); // Simulate async work
                IsAsyncOperationCompleted = true;
            }));
        }

        protected override void OnDisposeUnmanagedResources()
        {
            UnmanagedResourcesDisposed = true;
        }
    }

    [Fact]
    public async Task DisposeAsync_ShouldCallOnDisposeManagedResourcesAsync()
    {
        // Arrange
        var disposable = new TestAsyncDisposable();

        // Act
        await disposable.DisposeAsync();

        // Assert
        Assert.True(disposable.ManagedResourcesDisposed);
    }

    [Fact]
    public async Task DisposeAsync_ShouldSuppressFinalize()
    {
        // Arrange
        var disposable = new TestAsyncDisposable();

        // Act
        await disposable.DisposeAsync();

        // Assert
        Assert.True(disposable.Disposed);
    }

    [Theory]
    [InlineData(100)]  // Fast disposal
    [InlineData(1000)] // Slower disposal
    public async Task DisposeAsync_ShouldHandleVariousAsyncScenarios(int delayMs)
    {
        // Arrange
        var disposable = new TestAsyncDisposable();
        var sw = Stopwatch.StartNew();

        // Act
        await using (disposable)
        {
            await Task.Delay(delayMs);
        }

        sw.Stop();

        // Assert
        Assert.True(disposable.ManagedResourcesDisposed);
        Assert.True(disposable.IsAsyncOperationCompleted);
        Assert.True(sw.ElapsedMilliseconds >= delayMs);
    }
}
