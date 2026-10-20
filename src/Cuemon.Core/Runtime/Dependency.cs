using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Runtime;
/// <summary>
/// Represents the base class from which all implementations of dependency relationship to an object should derive.
/// </summary>
/// <remarks>The implementing class of the <see cref="Dependency"/> class must monitor the dependency relationships so that when any of them changes, action will automatically be taken.</remarks>
public abstract class Dependency : IDependency
{
    private IEnumerable<IWatcher> _watchers;
    private readonly Func<EventHandler<WatcherEventArgs>, IEnumerable<IWatcher>> _watchersHandler;
#if NET10_0_OR_GREATER
    private readonly System.Threading.Lock _lock = new();
#else
    private readonly object _lock = new();
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="Dependency" /> class.
    /// </summary>
    /// <param name="watchersHandler">The function delegate that associates watchers to this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="IWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    protected Dependency(Func<EventHandler<WatcherEventArgs>, IEnumerable<IWatcher>> watchersHandler, bool breakTieOnChanged)
    {
        Validator.ThrowIfNull(watchersHandler);
        BreakTieOnChanged = breakTieOnChanged;
        _watchersHandler = watchersHandler;
    }

    /// <summary>
    /// Occurs when a <see cref="Dependency"/> has changed.
    /// </summary>
    public event EventHandler<DependencyEventArgs> DependencyChanged;

    /// <summary>
    /// Gets a value indicating whether all <see cref="IWatcher"/> instances is disassociated with this dependency after first notification of changed.
    /// </summary>
    /// <value><c>true</c> if all <see cref="IWatcher"/> instances is disassociated with this dependency after first notification of changed; otherwise, <c>false</c>.</value>
    public bool BreakTieOnChanged { get; }

    /// <summary>
    /// Gets time when the dependency was last changed.
    /// </summary>
    /// <value>The time when the dependency was last changed.</value>
    /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
    public DateTime? UtcLastModified { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Dependency"/> object has changed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if the <see cref="Dependency"/> object has changed; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasChanged => UtcLastModified.HasValue;

    /// <summary>
    /// Marks the time when a dependency last changed.
    /// </summary>
    /// <param name="utcLastModified">The time when the dependency last changed.</param>
    protected void SetUtcLastModified(DateTime utcLastModified)
    {
        if (utcLastModified.Kind != DateTimeKind.Utc) { throw new ArgumentException("The time from when the dependency was last changed, must be specified in  Coordinated Universal Time (UTC).", nameof(utcLastModified)); }
        UtcLastModified = utcLastModified;
    }

    /// <summary>
    /// Raises the <see cref="DependencyChanged"/> event.
    /// </summary>
    /// <param name="e">The <see cref="DependencyEventArgs"/> instance containing the event data.</param>
    protected virtual void OnDependencyChangedRaised(DependencyEventArgs e)
    {
        var handler = DependencyChanged;
        handler?.Invoke(this, e);
    }

    /// <summary>
    /// Starts and performs the necessary dependency tasks of this instance.
    /// </summary>
    public virtual void Start()
    {
        StartAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts and performs the necessary dependency tasks of this instance.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual Task StartAsync()
    {
        _watchers = _watchersHandler(OnWatcherChanged);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when this object receives a signal from one or more of the associated <see cref="IWatcher"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="WatcherEventArgs"/> instance containing the event data.</param>
    protected virtual void OnWatcherChanged(object sender, WatcherEventArgs args)
    {
        var utcLastModified = DateTime.UtcNow;
        SetUtcLastModified(utcLastModified);
        if (BreakTieOnChanged && _watchers != null)
        {
            lock (_lock)
            {
                if (_watchers != null)
                {
                    foreach (var watcher in _watchers)
                    {
                        watcher.Changed -= OnWatcherChanged;
                        watcher.Dispose();
                    }
                }
                _watchers = null;
            }
        }
        OnDependencyChangedRaised(new DependencyEventArgs(utcLastModified));
    }
}
