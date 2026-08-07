using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Runtime;

namespace Cuemon.Net.Http;
/// <summary>
/// Provides a way to monitor any changes occurred to one or more URI resources while notifying subscribing objects.
/// </summary>
/// <seealso cref="Dependency" />
public class HttpDependency : Dependency
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpDependency"/> class.
    /// </summary>
    /// <param name="lazyFileWatcher">The <see cref="HttpWatcher"/> to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="HttpWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="lazyFileWatcher"/> cannot be null.
    /// </exception>
    /// <remarks>The <see cref="HttpWatcher"/> initialization is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public HttpDependency(Lazy<HttpWatcher> lazyFileWatcher, bool breakTieOnChanged = false) : this(Arguments.Yield(Validator.CheckParameter(lazyFileWatcher, () => Validator.ThrowIfNull(lazyFileWatcher))), breakTieOnChanged)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpDependency"/> class.
    /// </summary>
    /// <param name="lazyFileWatchers">The <see cref="HttpWatcher"/> sequence to associate with this dependency.</param>
    /// <param name="breakTieOnChanged">if set to <c>true</c> all <see cref="HttpWatcher"/> instances is disassociated with this dependency after first notification of changed.</param>
    /// <remarks>The sequence of <see cref="HttpWatcher"/> initializations is deferred until <see cref="Dependency.StartAsync"/> is invoked.</remarks>
    public HttpDependency(IEnumerable<Lazy<HttpWatcher>> lazyFileWatchers, bool breakTieOnChanged = false) : base(watcherChanged =>
    {
        var watchers = new List<HttpWatcher>();
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
