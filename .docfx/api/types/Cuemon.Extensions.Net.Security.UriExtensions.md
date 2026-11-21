---
uid: Cuemon.Extensions.Net.Security.UriExtensions
example:
- *content
---

The following example demonstrates how to sign and validate a <see cref="Uri" /> with <see cref="UriExtensions" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Extensions.Net.Security;

var secret = Encoding.UTF8.GetBytes("1234");
var location = new Uri("https://example.com/search?q=cuemon");
var signed = location.ToSignedUri(secret, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

signed.ValidateSignedUri(secret);

Console.WriteLine(signed != location);
Console.WriteLine(signed.AbsoluteUri);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6f3f7044c385dcea6327b1b7"><a href="~/run-actions/6f3f7044c385dcea6327b1b7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6f3f7044c385dcea6327b1b7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
