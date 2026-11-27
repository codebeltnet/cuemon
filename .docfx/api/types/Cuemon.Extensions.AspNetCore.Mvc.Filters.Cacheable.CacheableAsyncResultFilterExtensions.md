---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable.CacheableAsyncResultFilterExtensions
example:
- *content
---

The following example configures the same cache-filter collection patterns exercised by the unit tests: use the convenience methods for the default pipeline, or switch to the generic overloads when you need explicit ordering and option control, then inspect the configured filter count and ordering.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable.DocExamples;

var example = new CacheableAsyncResultFilterExtensionsExample();
example.Describe();
var customProfile = example.CreateCustomProfile();
Console.WriteLine($"Custom order: {string.Join(", ", customProfile.Filters.Select(f => f.GetType().Name))}");

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable.DocExamples
{
    public sealed class CacheableAsyncResultFilterExtensionsExample
    {
        public HttpCacheableOptions CreateDefaultProfile()
        {
            var options = new HttpCacheableOptions();
            options.Filters.AddLastModifiedHeader();
            options.Filters.AddEntityTagHeader();
            return options;
        }

        public HttpCacheableOptions CreateCustomProfile()
        {
            var options = new HttpCacheableOptions();
            options.Filters.AddFilter<HttpEntityTagHeaderFilter, HttpEntityTagHeaderOptions>(entityTag =>
            {
                entityTag.UseEntityTagResponseParser = true;
            });
            options.Filters.InsertFilter<HttpLastModifiedHeaderFilter, HttpLastModifiedHeaderOptions>(0);
            return options;
        }

        public void Describe()
        {
            var defaultProfile = CreateDefaultProfile();
            var customProfile = CreateCustomProfile();

            Console.WriteLine($"{defaultProfile.Filters.Count} default filters, {customProfile.Filters.Count} custom filters.");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4e54ac4c7bbddfb17a99040c"><a href="~/run-actions/4e54ac4c7bbddfb17a99040c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4e54ac4c7bbddfb17a99040c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
