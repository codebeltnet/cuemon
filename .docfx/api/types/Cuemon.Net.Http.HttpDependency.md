---
uid: Cuemon.Net.Http.HttpDependency
example:
- *content
---

`HttpDependency` wraps an `HttpWatcher` to monitor an HTTP resource for changes, detecting modifications via ETag and Last-Modified headers. This example creates a `Lazy<HttpWatcher>` that monitors `https://example.com/api/status` with HEAD requests at 30-second intervals, then constructs an `HttpDependency` from the factory and subscribes to the `DependencyChanged` event. Key steps include calling `StartAsync` to begin monitoring and handling the `DependencyChanged` event to react to changes. Console output prints the UTC timestamp when a resource change is detected.

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;

namespace MyApp.Net
{
    public class HttpDependencyExample
    {
        public async Task DemonstrateAsync()
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

            // Start monitoring
            await dependency.StartAsync();

            // The watcher will poll the URI every 30 seconds
            // and raise DependencyChanged when a change is detected

            Console.WriteLine("Monitoring started. Press any key to stop...");
            Console.ReadKey();
        }
    }
}
```
