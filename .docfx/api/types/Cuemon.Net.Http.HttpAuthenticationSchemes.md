---
uid: Cuemon.Net.Http.HttpAuthenticationSchemes
example:
- *content
---

The following example shows how to use the `HttpAuthenticationSchemes` constants to construct HTTP Authorization headers. It prints each scheme name and creates a Basic authentication header value.

```csharp
// Program.cs
using System;
using System.Net.Http.Headers;
using System.Text;
using Cuemon.Net.Http;

// Use scheme constants to construct Authorization headers
string basic = HttpAuthenticationSchemes.Basic;
string bearer = HttpAuthenticationSchemes.Bearer;
string digest = HttpAuthenticationSchemes.Digest;

Console.WriteLine($"Basic: {basic}");
Console.WriteLine($"Bearer: {bearer}");
Console.WriteLine($"Digest: {digest}");

// Example: create a Basic authentication header value
string credentials = Convert.ToBase64String(
    Encoding.UTF8.GetBytes("user:password"));
string authHeader = $"{basic} {credentials}";
Console.WriteLine($"Authorization: {authHeader}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b402f17819a76486beec2497"><a href="~/run-actions/b402f17819a76486beec2497/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b402f17819a76486beec2497/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
