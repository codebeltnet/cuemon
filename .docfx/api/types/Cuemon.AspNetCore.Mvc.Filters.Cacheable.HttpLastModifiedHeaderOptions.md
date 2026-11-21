---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderOptions
example:
- *content
---

The following example applies the default <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderOptions.LastModifiedProvider"/> to a timestamped response model. The sample supplies created and modified timestamps, applies the provider to a GET response, and prints the resulting Last-Modified header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

HttpLastModifiedHeaderOptionsExample.Demonstrate();

namespace MyApp.Examples
{
    public static class HttpLastModifiedHeaderOptionsExample
    {
        public static void Demonstrate()
        {
            var options = new HttpLastModifiedHeaderOptions();
            var context = new DefaultHttpContext();
            context.Request.Method = HttpMethods.Get;

            var timestamp = new SampleEntityDataTimestamp(
                DateTime.Parse("2024-01-01T00:00:00Z"),
                DateTime.Parse("2024-01-02T00:00:00Z"));

            options.LastModifiedProvider(timestamp, context);

            Console.WriteLine(options.HasLastModifiedProvider);
            Console.WriteLine(context.Response.Headers[HeaderNames.LastModified].ToString());
        }

        private sealed class SampleEntityDataTimestamp : IEntityDataTimestamp
        {
            public SampleEntityDataTimestamp(DateTime created, DateTime? modified)
            {
                Created = created;
                Modified = modified;
            }

            public DateTime Created { get; }

            public DateTime? Modified { get; }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5909ec077e6424ca5387ea47"><a href="~/run-actions/5909ec077e6424ca5387ea47/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5909ec077e6424ca5387ea47/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
