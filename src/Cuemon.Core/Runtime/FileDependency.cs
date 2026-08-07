using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Runtime;
/// <summary>
/// Provides a way to monitor any changes occurred to one or more files while notifying subscribing objects.
/// </summary>
/// <seealso cref="Dependency" />
public class FileDependency : Dependency
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileDependency"/> class.
    /// </summary>
    /// <param name="lazyFileWatcher">The <see cref="FileWatcher"/> to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="FileWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="lazyFileWatcher"/> cannot be null.
    /// </exception>
    /// <remarks>The <see cref="FileWatcher"/> initialization is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public FileDependency(Lazy<FileWatcher> lazyFileWatcher, bool breakTieOnChanged = false) : this(Arguments.Yield(Validator.CheckParameter(lazyFileWatcher, () => Validator.ThrowIfNull(lazyFileWatcher))), breakTieOnChanged)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDependency"/> class.
    /// </summary>
    /// <param name="lazyFileWatchers">The <see cref="FileWatcher"/> sequence to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="FileWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <remarks>The sequence of <see cref="FileWatcher"/> initializations is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public FileDependency(IEnumerable<Lazy<FileWatcher>> lazyFileWatchers, bool breakTieOnChanged = false) : base(watcherChanged =>
    {
        var watchers = new List<FileWatcher>();
        foreach (var lazyFileWatcher in lazyFileWatchers.Select(lazy => lazy.Value))
        {
            var fileWatcher = lazyFileWatcher;
            fileWatcher.Changed += watcherChanged;
            fileWatcher.StartMonitoring();
            watchers.Add(fileWatcher);
        }
        return watchers;
    }, breakTieOnChanged)
    {
    }
}
