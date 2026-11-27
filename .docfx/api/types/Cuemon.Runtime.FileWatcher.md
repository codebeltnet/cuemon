---
uid: Cuemon.Runtime.FileWatcher
example:
- *content
---

The following example shows how to start a file watcher, observe change notifications, and pause signaling again.

```csharp
// Program.cs
using System;
using System.IO;
using System.Threading;
using Cuemon.Runtime;

string directory = Path.Combine(Path.GetTempPath(), "cuemon-docs");
Directory.CreateDirectory(directory);
string filePath = Path.Combine(directory, "health.txt");
File.WriteAllText(filePath, "ready");

using var watcher = new FileWatcher(filePath, readFile: true, options =>
{
    options.DueTime = TimeSpan.Zero;
    options.Period = TimeSpan.FromSeconds(5);
    options.DueTimeOnChanged = TimeSpan.FromMilliseconds(250);
});

watcher.Changed += static (_, e) => Console.WriteLine($"{e.UtcLastModified:O} ({e.Delayed.TotalMilliseconds} ms)");
watcher.StartMonitoring();

Console.WriteLine(watcher.Path);
Console.WriteLine(watcher.ReadFile);
Console.WriteLine(watcher.UtcCreated.ToString("O"));

watcher.ChangeSignaling(Timeout.InfiniteTimeSpan);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="296181754f9fd3e46901c11f"><a href="~/run-actions/296181754f9fd3e46901c11f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/296181754f9fd3e46901c11f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
