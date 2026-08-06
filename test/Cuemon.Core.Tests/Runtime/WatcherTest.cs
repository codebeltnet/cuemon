using System;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Runtime;
public class WatcherTest : Test
{
    public WatcherTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SetUtcLastModified_WithLocalTime_ThrowsArgumentException()
    {
        var sut = new FakeWatcher();

        Assert.Throws<ArgumentException>(() => sut.MarkLastModified(DateTime.Now));
    }

    [Fact]
    public void SetUtcLastModified_WithFutureUtcTime_UpdatesUtcLastModified()
    {
        var sut = new FakeWatcher();
        var expected = DateTime.UtcNow.AddMinutes(5);

        sut.MarkLastModified(expected);

        Assert.Equal(expected, sut.UtcLastModified);
    }

    [Fact]
    public void OnChangedRaised_WithNoDelay_RaisesChangedImmediately()
    {
        var sut = new FakeWatcher();
        var expected = DateTime.UtcNow;
        WatcherEventArgs eventArgs = null;
        sut.Changed += (sender, args) => eventArgs = args;
        sut.MarkLastModified(expected);

        sut.RaiseChangedEvent();

        Assert.NotNull(eventArgs);
        Assert.Equal(expected, eventArgs.UtcLastModified);
        Assert.Equal(TimeSpan.Zero, eventArgs.Delayed);
    }

    [Fact]
    public void OnChangedRaised_WithDelay_RaisesChangedOnceAfterPostponement()
    {
        var signal = new ManualResetEventSlim(false);
        try
        {
            var delay = TimeSpan.FromMilliseconds(100);
            var sut = new FakeWatcher(o => o.DueTimeOnChanged = delay);
            var expected = DateTime.UtcNow;
            var count = 0;
            WatcherEventArgs eventArgs = null;
            sut.Changed += (sender, args) =>
            {
                Interlocked.Increment(ref count);
                eventArgs = args;
                signal.Set();
            };
            sut.MarkLastModified(expected);

            sut.RaiseChangedEvent();
            sut.RaiseChangedEvent();

            Assert.True(signal.Wait(TimeSpan.FromSeconds(5)));
            Thread.Sleep(150);
            Assert.Equal(1, count);
            Assert.NotNull(eventArgs);
            Assert.Equal(expected, eventArgs.UtcLastModified);
            Assert.Equal(delay, eventArgs.Delayed);
            sut.Dispose();
        }
        finally
        {
            signal.Dispose();
        }
    }

    [Fact]
    public void ChangeSignaling_WithDueTimeOnly_PreservesExistingPeriodAndSignalsWatcher()
    {
        var signal = new ManualResetEventSlim(false);
        try
        {
            var expectedPeriod = TimeSpan.FromMinutes(1);
            var sut = new FakeWatcher(o =>
            {
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = expectedPeriod;
            }, watcher => signal.Set());

            sut.StartMonitoring();
            sut.ChangeSignaling(TimeSpan.Zero);

            Assert.True(signal.Wait(TimeSpan.FromSeconds(5)));
            Assert.Equal(TimeSpan.Zero, sut.CurrentDueTime);
            Assert.Equal(expectedPeriod, sut.CurrentPeriod);
            Assert.True(sut.UtcLastSignaled > DateTime.MinValue);
            sut.Dispose();
        }
        finally
        {
            signal.Dispose();
        }
    }

    [Fact]
    public void ChangeSignaling_WithDueTimeAndPeriod_UpdatesSettingsAndSignalsWatcher()
    {
        var signal = new ManualResetEventSlim(false);
        try
        {
            var dueTime = TimeSpan.FromMilliseconds(10);
            var period = TimeSpan.FromMilliseconds(50);
            var sut = new FakeWatcher(o =>
            {
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            }, watcher => signal.Set());

            sut.StartMonitoring();
            sut.ChangeSignaling(dueTime, period);

            Assert.True(signal.Wait(TimeSpan.FromSeconds(5)));
            Assert.Equal(dueTime, sut.CurrentDueTime);
            Assert.Equal(period, sut.CurrentPeriod);
            Assert.True(sut.SignalCount > 0);
            sut.Dispose();
        }
        finally
        {
            signal.Dispose();
        }
    }

    [Fact]
    public void Dispose_AfterMonitoring_CanBeCalledMultipleTimes()
    {
        var sut = new FakeWatcher(o =>
        {
            o.DueTime = Timeout.InfiniteTimeSpan;
            o.Period = Timeout.InfiniteTimeSpan;
        });

        sut.StartMonitoring();
        sut.Dispose();
        sut.Dispose();

        Assert.True(sut.Disposed);
    }

    private sealed class FakeWatcher : Watcher
    {
        private readonly Action<FakeWatcher> _onSignaled;

        public FakeWatcher(Action<WatcherOptions> setup = null, Action<FakeWatcher> onSignaled = null) : base(setup)
        {
            _onSignaled = onSignaled;
        }

        public TimeSpan CurrentDueTime => DueTime;

        public TimeSpan CurrentPeriod => Period;

        public int SignalCount { get; private set; }

        public void MarkLastModified(DateTime value)
        {
            SetUtcLastModified(value);
        }

        public void RaiseChangedEvent()
        {
            OnChangedRaised();
        }

        protected override Task HandleSignalingAsync()
        {
            SignalCount++;
            _onSignaled?.Invoke(this);
            return Task.CompletedTask;
        }
    }
}
