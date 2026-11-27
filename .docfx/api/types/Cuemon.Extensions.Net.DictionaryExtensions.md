---
uid: Cuemon.Extensions.Net.DictionaryExtensions
example:
- *content
---

`DictionaryExtensions` in the `Net` namespace converts `Dictionary<string, string[]>` into URL query strings with optional percent-encoding. This example creates a dictionary with parameters `"search": ["dotnet"]`, `"page": ["1"]`, and `"tags": ["aspnet", "core"]`, then calls `ToQueryString()` to produce `?search=dotnet&page=1&tags=aspnet&tags=core` and `ToQueryString(urlEncode: true)` for URL-encoded output. It also handles an empty dictionary that returns an empty string. Console output shows the raw query string, the encoded variant, and the empty-collection result `"Empty: ''"`.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Net;

// Build a query string from a dictionary
var parameters = new Dictionary<string, string[]>
{
    { "search", new[] { "dotnet" } },
    { "page", new[] { "1" } },
    { "tags", new[] { "aspnet", "core" } }
};

// Convert to a query string (not URL-encoded)
string queryString = parameters.ToQueryString();
Console.WriteLine(queryString);
// Output: ?search=dotnet&page=1&tags=aspnet&tags=core

// URL-encode the values
string encoded = parameters.ToQueryString(urlEncode: true);
Console.WriteLine(encoded);
// Output: ?search=dotnet&page=1&tags=aspnet&tags=core

// Useful for building API request URLs
var empty = new Dictionary<string, string[]>();
string emptyQs = empty.ToQueryString();
Console.WriteLine($"Empty: '{emptyQs}'"); // Empty: ''
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="63a8ddddaba7e83372baf088"><a href="~/run-actions/63a8ddddaba7e83372baf088/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/63a8ddddaba7e83372baf088/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
