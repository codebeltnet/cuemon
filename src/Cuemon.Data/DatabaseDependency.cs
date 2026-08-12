using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Runtime;

namespace Cuemon.Data;
/// <summary>
/// Provides a way to monitor any changes occurred to one or more relational data sources while notifying subscribing objects.
/// </summary>
/// <seealso cref="Dependency" />
public class DatabaseDependency : Dependency
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseDependency"/> class.
    /// </summary>
    /// <param name="lazyDatabaseWatcher">The <see cref="DatabaseWatcher"/> to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="DatabaseWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="lazyDatabaseWatcher"/> cannot be null.
    /// </exception>
    /// <remarks>The <see cref="DatabaseWatcher"/> initialization is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public DatabaseDependency(Lazy<DatabaseWatcher> lazyDatabaseWatcher, bool breakTieOnChanged = false) : this(Arguments.Yield(Validator.CheckParameter(lazyDatabaseWatcher, () => Validator.ThrowIfNull(lazyDatabaseWatcher))), breakTieOnChanged)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseDependency"/> class.
    /// </summary>
    /// <param name="lazyDatabaseWatchers">The <see cref="DatabaseWatcher"/> sequence to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="DatabaseWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <remarks>The sequence of <see cref="DatabaseWatcher"/> initializations is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public DatabaseDependency(IEnumerable<Lazy<DatabaseWatcher>> lazyDatabaseWatchers, bool breakTieOnChanged = false) : base(watcherChanged =>
    {
        var watchers = new List<DatabaseWatcher>();
        foreach (var lazyDatabaseWatcher in lazyDatabaseWatchers.Select(lazy => lazy.Value))
        {
            lazyDatabaseWatcher.Changed += watcherChanged;
            lazyDatabaseWatcher.StartMonitoring();
            watchers.Add(lazyDatabaseWatcher);
        }
        return watchers;
    }, breakTieOnChanged)
    {
    }
}
