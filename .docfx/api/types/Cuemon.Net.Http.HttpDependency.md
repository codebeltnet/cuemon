---
uid: Cuemon.Net.Http.HttpDependency
example:
- *content
---

`HttpDependency` wraps an `HttpWatcher` to monitor an HTTP resource for changes, detecting modifications via ETag and Last-Modified headers. This example creates a `Lazy<HttpWatcher>` that monitors `https://example.com/api/status` with HEAD requests at 30-second intervals, then constructs an `HttpDependency` from the factory and subscribes to the `DependencyChanged` event (ready to begin polling when `StartAsync` is called). Console output confirms that the `HttpDependency` has been configured for the target URI.

```csharp
// Program.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;
using MyApp.Net;

new HttpDependencyExample().Demonstrate();

namespace MyApp.Net
{
    public class HttpDependencyExample
    {
        public void Demonstrate()
        {
            // Create an HttpWatcher that monitors changes to a URI resource
            var watcherFactory = new Lazy<HttpWatcher>(() =>
            {
                var location = new Uri("https://example.com/api/status");
                return new HttpWatcher(location, o =>
                {
                    o.ReadResponseBody = false; // use HEAD requests (checks ETag/Last-Modified)
                    o.Period = TimeSpan.FromSeconds(30);
                });
            });

            // Create a dependency that wraps the watcher
            var dependency = new HttpDependency(watcherFactory);

            // Subscribe to change notifications
            dependency.DependencyChanged += (sender, args) =>
            {
                Console.WriteLine($"Resource changed at: {args.UtcLastModified}");
            };

            // Call StartAsync when you are ready to begin polling the URI.
            // The watcher will poll every 30 seconds and raise DependencyChanged when a change is detected.
            Console.WriteLine("HttpDependency configured for https://example.com/api/status.");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="326670a2521fcdd5b4b2d6b6"><a href="~/run-actions/326670a2521fcdd5b4b2d6b6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/326670a2521fcdd5b4b2d6b6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
