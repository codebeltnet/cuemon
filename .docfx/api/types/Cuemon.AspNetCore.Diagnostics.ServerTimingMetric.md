---
uid: Cuemon.AspNetCore.Diagnostics.ServerTimingMetric
example:
- *content
---

`ServerTimingMetric` records performance data points for the HTTP `Server-Timing` header, supporting metrics with or without duration and description. This example constructs three metrics — `"db-query"` with 135.2ms duration and a description, `"cache-hit"` as a marker-only metric, and `"redis-get"` with a 3.7ms duration — then integrates them with `IServerTiming` via `AddServerTiming` to build a complete server-timing collection. Key steps include creating metrics with various constructor overloads and iterating the final list. Console output shows header-value strings such as `db-query;dur=135.2;desc="Customer order lookup"` and `cache-hit`.

```csharp
using System;
using Cuemon.AspNetCore.Diagnostics;

namespace MyApp.Diagnostics
{
    public class ServerTimingMetricExample
    {
        public void Demonstrate()
        {
            // Record a metric for a database query
            var dbMetric = new ServerTimingMetric("db-query",
                TimeSpan.FromMilliseconds(135.2),
                "Customer order lookup");

            Console.WriteLine($"Name: {dbMetric.Name}");
            Console.WriteLine($"Duration: {dbMetric.Duration}ms");
            Console.WriteLine($"Description: {dbMetric.Description}");
            Console.WriteLine($"Header value: {dbMetric}");
            // Output: db-query;dur=135.2;desc="Customer order lookup"

            // Record a metric without duration (marker only)
            var marker = new ServerTimingMetric("cache-hit");
            Console.WriteLine(marker);
            // Output: cache-hit

            // Record a metric without description
            var fastMetric = new ServerTimingMetric("redis-get",
                TimeSpan.FromMilliseconds(3.7));
            Console.WriteLine(fastMetric);
            // Output: redis-get;dur=3.7

            // Use with IServerTiming to add to response
            IServerTiming serverTiming = new ServerTiming();
            serverTiming
                .AddServerTiming("auth", TimeSpan.FromMilliseconds(12.5), "Token validation")
                .AddServerTiming("sql", TimeSpan.FromMilliseconds(89.1), "Product search");

            foreach (var metric in serverTiming.Metrics)
            {
                Console.WriteLine(metric);

}}}
}

```
