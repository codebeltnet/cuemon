---
uid: Cuemon.Extensions.Collections.Specialized.DictionaryExtensions
example:
- *content
---

`DictionaryExtensions` in the `Specialized` namespace converts `Dictionary<string, string[]>` into `NameValueCollection` instances, joining multi-valued keys with a configurable delimiter. This example creates a dictionary with `"colors": ["red", "green", "blue"]` and `"sizes": ["small", "large"]`, calls `ToNameValueCollection()` to produce a collection using the default comma delimiter, then calls `ToNameValueCollection` with a custom `";"` delimiter via an options delegate. Console output confirms that `"colors"` becomes `"red,green,blue"` with the default delimiter and `"red;green;blue"` with the custom semicolon delimiter.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon;
using Cuemon.Extensions.Collections.Specialized;

// Create a dictionary with string array values
var source = new Dictionary<string, string[]>
{
    ["colors"] = new[] { "red", "green", "blue" },
    ["sizes"] = new[] { "small", "large" }
};

// Convert to a NameValueCollection (default delimiter is comma)
NameValueCollection nvc = source.ToNameValueCollection();

Console.WriteLine(nvc["colors"]); // "red,green,blue"
Console.WriteLine(nvc["sizes"]);  // "small,large"

// Convert with a custom delimiter
NameValueCollection nvcSemicolon = source.ToNameValueCollection(o =>
{
    o.Delimiter = ";";
});

Console.WriteLine(nvcSemicolon["colors"]); // "red;green;blue"
Console.WriteLine(nvcSemicolon["sizes"]);  // "small;large"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a93f8d9a5cf54715cd7847c5"><a href="~/run-actions/a93f8d9a5cf54715cd7847c5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a93f8d9a5cf54715cd7847c5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
