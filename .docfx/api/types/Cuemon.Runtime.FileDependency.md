---
uid: Cuemon.Runtime.FileDependency
example:
- *content
---

`FileDependency` defers `FileWatcher` creation until monitoring begins by accepting a `Lazy<FileWatcher>` factory. This example creates a temporary `settings.json` file, wraps a `FileWatcher` in a `Lazy<>` with a 500ms polling period, and passes it to `FileDependency` with `breakTieOnChanged: true`. Key steps include checking `lazyWatcher.IsValueCreated` before and after `StartAsync` to confirm deferred creation, subscribing to `DependencyChanged`, and inspecting `HasChanged` after signaling. Console output shows the watcher creation status, `BreakTieOnChanged` value, and the changed state.

```csharp
// Program.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.Runtime;

string directory = Path.Combine(Path.GetTempPath(), "cuemon-docs");
Directory.CreateDirectory(directory);
string filePath = Path.Combine(directory, "settings.json");
File.WriteAllText(filePath, "{ }");

var lazyWatcher = new Lazy<FileWatcher>(() => new FileWatcher(filePath, false, options =>
{
    options.Period = TimeSpan.FromMilliseconds(500);
}));

var dependency = new FileDependency(lazyWatcher, breakTieOnChanged: true);
dependency.DependencyChanged += static (_, e) => Console.WriteLine(e.UtcLastModified.ToString("O"));

Console.WriteLine(lazyWatcher.IsValueCreated);
Console.WriteLine(dependency.BreakTieOnChanged);

await dependency.StartAsync();

Console.WriteLine(lazyWatcher.IsValueCreated);
Console.WriteLine(dependency.HasChanged);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5c2d665e0a7c6e7ced10395f"><a href="~/run-actions/5c2d665e0a7c6e7ced10395f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5c2d665e0a7c6e7ced10395f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
