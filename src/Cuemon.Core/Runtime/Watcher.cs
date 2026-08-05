using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Runtime
{
    /// <summary>
    /// Represents the base class from which all implementations of resource monitoring should derive.
    /// </summary>
    public abstract class Watcher : Disposable, IWatcher
    {
#if NET9_0_OR_GREATER
        private readonly Lock _lock = new();
#else
        private readonly object _lock = new();
#endif
        private Timer _watcherTimer;
        private Timer _watcherPostponingTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher" /> class.
        /// </summary>
        /// <param name="setup">The <see cref="WatcherOptions" /> which needs to be configured.</param>
        protected Watcher(Action<WatcherOptions> setup)
        {
            var options = Patterns.Configure(setup);
            DueTime = options.DueTime;
            Period = options.Period;
            DueTimeOnChanged = options.DueTimeOnChanged;
            UtcLastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Occurs when a resource has changed.
        /// </summary>
        public event EventHandler<WatcherEventArgs> Changed;

        /// <summary>
        /// Gets the time when the resource being monitored was last changed.
        /// </summary>
        /// <value>The time when the resource being monitored was last changed.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastModified { get; private set; }

        /// <summary>
        /// Gets the time when the last signaling occurred.
        /// </summary>
        /// <value>The time when the last signaling occurred.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastSignaled { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling.</value>
        protected TimeSpan DueTime { get; private set; }

        /// <summary>
        /// Gets the time interval between periodic signaling.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the time interval between periodic signaling.</value>
        protected TimeSpan Period { get; private set; }

        /// <summary>
        /// Gets the amount of time to postpone a <see cref="Changed"/> event.
        /// </summary>
        protected TimeSpan DueTimeOnChanged { get; }

        /// <summary>
        /// Starts the timer that will monitor this <see cref="Watcher"/> implementation.
        /// </summary>
        public void StartMonitoring()
        {
            lock (_lock)
            {
                _watcherTimer ??= TimerFactory.CreateNonCapturingTimer(TimerInvoking, null, DueTime, Period);
            }
        }

        /// <summary>
        /// Changes the signaling timer of the <see cref="Watcher"/>.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// If <paramref name="dueTime"/> is zero (0), the signaling is started immediately. If <paramref name="dueTime"/> is negative one (-1) milliseconds, the signaling is never started; and the underlying timer is disabled, but can be re-enabled by specifying a positive value for <paramref name="dueTime"/>.
        public void ChangeSignaling(TimeSpan dueTime)
        {
            ChangeSignaling(dueTime, Period);
        }

        /// <summary>
        /// Changes the signaling timer of the <see cref="Watcher"/>.
        /// </summary>
        /// <param name="dueTime">A <see cref="TimeSpan"/> representing the amount of time to delay before the <see cref="Watcher"/> starts signaling. Specify negative one (-1) milliseconds to prevent the signaling from starting. Specify zero (0) to start the signaling immediately.</param>
        /// <param name="period">The time interval between periodic signaling. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <remarks>If <paramref name="dueTime" /> is zero (0), the signaling is started immediately. If <paramref name="dueTime" /> is negative one (-1) milliseconds, the signaling is never started; and the underlying timer is disabled, but can be re-enabled by specifying a positive value for <paramref name="dueTime" />.
        /// If <paramref name="period" /> is zero (0) or negative one (-1) milliseconds, and <paramref name="dueTime" /> is positive, the signaling is done once; the periodic behavior of the underlying timer is disabled, but can be re-enabled by specifying a value greater than zero for <paramref name="period" />.</remarks>
        public virtual void ChangeSignaling(TimeSpan dueTime, TimeSpan period)
        {
            DueTime = dueTime;
            Period = period;
            _watcherTimer.Change(dueTime, period);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            _watcherTimer?.Dispose();
            _watcherPostponingTimer?.Dispose();
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(System.Boolean)" /> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeUnmanagedResources()
        {
            _watcherTimer = null;
            _watcherPostponingTimer = null;
        }

        /// <summary>
        /// Marks the time when a resource being monitored was last changed.
        /// </summary>
        /// <param name="utcLastModified">The time when a resource being monitored was last changed.</param>
        protected void SetUtcLastModified(DateTime utcLastModified)
        {
            if (utcLastModified.Kind != DateTimeKind.Utc) { throw new ArgumentException("The time from when the resource being monitored was last changed, must be specified in the Coordinated Universal Time (UTC).", nameof(utcLastModified)); }
            UtcLastModified = utcLastModified;
        }

        private void TimerInvoking(object o)
        {
            UtcLastSignaled = DateTime.UtcNow;
            HandleSignaling();
        }

        /// <summary>
        /// Handles the signaling of this <see cref="Watcher"/>.
        /// </summary>
        protected virtual void HandleSignaling()
        {
            HandleSignalingAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Handles the signaling of this <see cref="Watcher"/>.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected abstract Task HandleSignalingAsync();

        private void PostponedHandleSignaling(object parameter)
        {
            OnChangedRaisedCore(parameter as WatcherEventArgs);
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        /// <remarks>This method raises the <see cref="Changed"/> event with <see cref="UtcLastModified"/> and <see cref="DueTimeOnChanged"/> passed to a new instance of <see cref="WatcherEventArgs"/>.</remarks>
        protected void OnChangedRaised()
        {
            OnChangedRaised(new WatcherEventArgs(UtcLastModified, DueTimeOnChanged));
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="WatcherEventArgs"/> instance containing the event data.</param>
        protected virtual void OnChangedRaised(WatcherEventArgs e)
        {
            if (_watcherPostponingTimer != null) { return; } // we already have a postponed signaling
            if (DueTimeOnChanged != TimeSpan.Zero)
            {
                lock (_lock)
                {
                    _watcherPostponingTimer ??= TimerFactory.CreateNonCapturingTimer(PostponedHandleSignaling, e, DueTimeOnChanged, Timeout.InfiniteTimeSpan);
                }
                return;
            }
            OnChangedRaisedCore(e);
        }

        private void OnChangedRaisedCore(WatcherEventArgs e)
        {
            var handler = Changed;
            handler?.Invoke(this, e);
        }
    }
}