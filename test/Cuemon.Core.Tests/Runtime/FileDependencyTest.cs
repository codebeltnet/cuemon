using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Runtime;
public class FileDependencyTest : Test
{
    private static readonly TimeSpan PollingPeriod = TimeSpan.FromMilliseconds(200);
    private static readonly TimeSpan SignalTimeout = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan NoAdditionalSignalTimeout = TimeSpan.FromSeconds(2);

    public FileDependencyTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldNotInitializeFileWatcher()
    {
        var testDirectory = CreateTestDirectory();
        var filePath = Path.Combine(testDirectory, "UnitTest1.txt");
        var watcherFactory = new Lazy<FileWatcher>(() => new FileWatcher(filePath));
        var dependency = new FileDependency(watcherFactory);

        try
        {
            File.WriteAllText(filePath, "Unit Test is key to ensure high code quality.");

            Assert.False(watcherFactory.IsValueCreated);
            Assert.False(dependency.HasChanged);
            Assert.Null(dependency.UtcLastModified);
        }
        finally
        {
            if (watcherFactory.IsValueCreated) { watcherFactory.Value.Dispose(); }
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task StartAsync_ShouldReceiveTwoSignalsFromFileWatcher()
    {
        var testDirectory = CreateTestDirectory();
        var filePath = Path.Combine(testDirectory, "UnitTest2.txt");
        var watcherFactory = new Lazy<FileWatcher>(() => new FileWatcher(filePath, false, o =>
        {
            o.DueTime = Timeout.InfiniteTimeSpan;
            o.Period = Timeout.InfiniteTimeSpan;
        }));
        var dependency = new FileDependency(watcherFactory);
        var startedAt = DateTime.UtcNow;
        var signalTimes = new ConcurrentQueue<DateTime>();
        var firstSignal = new TaskCompletionSource<DateTime>(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondSignal = new TaskCompletionSource<DateTime>(TaskCreationOptions.RunContinuationsAsynchronously);
        var signalCount = 0;
        var dependencyChangedHandler = new EventHandler<DependencyEventArgs>((s, e) =>
        {
            signalTimes.Enqueue(e.UtcLastModified);
            switch (Interlocked.Increment(ref signalCount))
            {
                case 1:
                    firstSignal.TrySetResult(e.UtcLastModified);
                    break;
                case 2:
                    secondSignal.TrySetResult(e.UtcLastModified);
                    break;
            }
        });

        try
        {
            var initialLastWriteTime = WriteTextAndGetLastWriteTimeUtc(filePath, "Initial file content.");

            dependency.DependencyChanged += dependencyChangedHandler;

            await dependency.StartAsync();

            var firstChangeBaseline = initialLastWriteTime > watcherFactory.Value.UtcLastModified ? initialLastWriteTime : watcherFactory.Value.UtcLastModified;
            var firstLastWriteTime = WriteTextAndAdvanceLastWriteTimeUtc(filePath, "First file change.", firstChangeBaseline);
            watcherFactory.Value.ChangeSignaling(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
            var firstSignalTime = await WaitOrThrowAsync(firstSignal.Task, SignalTimeout);
            var secondLastWriteTime = WriteTextAndAdvanceLastWriteTimeUtc(filePath, "Second file change.", firstLastWriteTime);
            watcherFactory.Value.ChangeSignaling(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
            var secondSignalTime = await WaitOrThrowAsync(secondSignal.Task, SignalTimeout);
            var observedSignalTimes = signalTimes.ToArray();

            TestOutput.WriteLine(string.Join(Environment.NewLine, observedSignalTimes));

            Assert.True(firstLastWriteTime > initialLastWriteTime);
            Assert.True(secondLastWriteTime > firstLastWriteTime);
            Assert.True(watcherFactory.IsValueCreated);
            Assert.True(dependency.HasChanged);
            Assert.NotNull(dependency.UtcLastModified);
            Assert.InRange(firstSignalTime, startedAt, startedAt.AddSeconds(15));
            Assert.InRange(secondSignalTime, startedAt, startedAt.AddSeconds(15));
            Assert.InRange(dependency.UtcLastModified.Value, startedAt, startedAt.AddSeconds(15));
            Assert.Equal(2, observedSignalTimes.Length);
            Assert.Equal(secondSignalTime, dependency.UtcLastModified.Value);
        }
        finally
        {
            dependency.DependencyChanged -= dependencyChangedHandler;
            if (watcherFactory.IsValueCreated) { watcherFactory.Value.Dispose(); }
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task StartAsync_ShouldReceiveOnlyOneSignalFromFileWatcher()
    {
        var testDirectory = CreateTestDirectory();
        var filePath = Path.Combine(testDirectory, "UnitTest3.txt");
        var watcherFactory = new Lazy<FileWatcher>(() => new FileWatcher(filePath, false, o =>
        {
            o.DueTime = Timeout.InfiniteTimeSpan;
            o.Period = PollingPeriod;
        }));
        var dependency = new FileDependency(watcherFactory, true);
        var startedAt = DateTime.UtcNow;
        var signalTimes = new ConcurrentQueue<DateTime>();
        var firstSignal = new TaskCompletionSource<DateTime>(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondSignal = new TaskCompletionSource<DateTime>(TaskCreationOptions.RunContinuationsAsynchronously);
        var signalCount = 0;
        var dependencyChangedHandler = new EventHandler<DependencyEventArgs>((s, e) =>
        {
            signalTimes.Enqueue(e.UtcLastModified);
            switch (Interlocked.Increment(ref signalCount))
            {
                case 1:
                    firstSignal.TrySetResult(e.UtcLastModified);
                    break;
                case 2:
                    secondSignal.TrySetResult(e.UtcLastModified);
                    break;
            }
        });

        try
        {
            var initialLastWriteTime = WriteTextAndGetLastWriteTimeUtc(filePath, "Initial file content.");

            dependency.DependencyChanged += dependencyChangedHandler;

            await dependency.StartAsync();

            var firstChangeBaseline = initialLastWriteTime > watcherFactory.Value.UtcLastModified ? initialLastWriteTime : watcherFactory.Value.UtcLastModified;
            var firstLastWriteTime = WriteTextAndAdvanceLastWriteTimeUtc(filePath, "First file change.", firstChangeBaseline);
            watcherFactory.Value.ChangeSignaling(TimeSpan.Zero, PollingPeriod);
            var firstSignalTime = await WaitOrThrowAsync(firstSignal.Task, SignalTimeout);
            var secondLastWriteTime = WriteTextAndAdvanceLastWriteTimeUtc(filePath, "Second file change.", firstLastWriteTime);
            var receivedAdditionalSignal = await CompletesWithinAsync(secondSignal.Task, NoAdditionalSignalTimeout);
            var observedSignalTimes = signalTimes.ToArray();

            TestOutput.WriteLine(string.Join(Environment.NewLine, observedSignalTimes));

            Assert.True(firstLastWriteTime > initialLastWriteTime);
            Assert.True(secondLastWriteTime > firstLastWriteTime);
            Assert.False(receivedAdditionalSignal);
            Assert.True(watcherFactory.IsValueCreated);
            Assert.True(dependency.HasChanged);
            Assert.NotNull(dependency.UtcLastModified);
            Assert.InRange(firstSignalTime, startedAt, startedAt.AddSeconds(15));
            Assert.InRange(dependency.UtcLastModified.Value, startedAt, startedAt.AddSeconds(15));
            Assert.Equal(1, observedSignalTimes.Length);
            Assert.Equal(firstSignalTime, dependency.UtcLastModified.Value);
        }
        finally
        {
            dependency.DependencyChanged -= dependencyChangedHandler;
            if (watcherFactory.IsValueCreated) { watcherFactory.Value.Dispose(); }
            DeleteTestDirectory(testDirectory);
        }
    }

    private static string CreateTestDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "cuemon", "file-dependency", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteTestDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    private static DateTime WriteTextAndGetLastWriteTimeUtc(string path, string content)
    {
        File.WriteAllText(path, content);
        return File.GetLastWriteTimeUtc(path);
    }

    private static DateTime WriteTextAndAdvanceLastWriteTimeUtc(string path, string content, DateTime previousLastWriteTime)
    {
        File.WriteAllText(path, content);

        var currentLastWriteTime = File.GetLastWriteTimeUtc(path);
        if (currentLastWriteTime > previousLastWriteTime)
        {
            return currentLastWriteTime;
        }

        var candidateLastWriteTime = previousLastWriteTime.AddSeconds(2);
        for (var attempt = 0; attempt < 5; attempt++)
        {
            File.SetLastWriteTimeUtc(path, candidateLastWriteTime);
            currentLastWriteTime = File.GetLastWriteTimeUtc(path);
            if (currentLastWriteTime > previousLastWriteTime)
            {
                return currentLastWriteTime;
            }

            candidateLastWriteTime = candidateLastWriteTime.AddSeconds(2);
        }

        throw new InvalidOperationException("Unable to advance the file last-write timestamp.");
    }

    private static async Task<T> WaitOrThrowAsync<T>(Task<T> task, TimeSpan timeout)
    {
        var timeoutTask = Task.Delay(timeout);
        if (await Task.WhenAny(task, timeoutTask) != task) { throw new TimeoutException(); }
        return await task;
    }

    private static async Task<bool> CompletesWithinAsync(Task task, TimeSpan timeout)
    {
        var timeoutTask = Task.Delay(timeout);
        return await Task.WhenAny(task, timeoutTask) == task;
    }
}
