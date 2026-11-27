---
uid: Cuemon.Extensions.Net.NameValueCollectionExtensions
example:
- *content
---

`NameValueCollectionExtensions` in the `Net` namespace converts `NameValueCollection` instances into URL query strings with optional percent-encoding. This example populates a collection with `"name": "John Doe"`, `"city": "Copenhagen"`, and duplicate key `"hobbies": ["reading", "coding"]`, then calls `ToQueryString()` to produce `?name=John Doe&city=Copenhagen&hobbies=reading&hobbies=coding` and `ToQueryString(urlEncode: true)` where spaces become `+`. It also handles an empty collection that returns an empty string. Console output shows both query string variants and the empty result.

```csharp
// Program.cs
using System;
using System.Collections.Specialized;
using Cuemon.Extensions.Net;

var nvc = new NameValueCollection
{
    { "name", "John Doe" },
    { "city", "Copenhagen" },
    { "hobbies", "reading" },
    { "hobbies", "coding" }
};

// Convert NameValueCollection to a query string (not URL-encoded)
string queryString = nvc.ToQueryString();
Console.WriteLine(queryString);
// Output: ?name=John Doe&city=Copenhagen&hobbies=reading&hobbies=coding

// URL-encode the values for safe HTTP transmission
string encoded = nvc.ToQueryString(urlEncode: true);
Console.WriteLine(encoded);
// Output: ?name=John+Doe&city=Copenhagen&hobbies=reading&hobbies=coding

// Empty collection
var empty = new NameValueCollection();
string emptyResult = empty.ToQueryString();
Console.WriteLine($"Empty: '{emptyResult}'"); // Empty: ''
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="930ac26fa3b04843ddf83582"><a href="~/run-actions/930ac26fa3b04843ddf83582/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/930ac26fa3b04843ddf83582/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
