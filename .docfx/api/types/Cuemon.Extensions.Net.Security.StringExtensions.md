---
uid: Cuemon.Extensions.Net.Security.StringExtensions
example:
- *content
---

The following example demonstrates how to sign a URI string and validate the signature later.

```csharp
// Program.cs
using System;
using System.Security;
using System.Text;
using Cuemon.Extensions.Net.Security;

var secret = Encoding.UTF8.GetBytes("1234");
var uriString = "https://example.com/search?q=cuemon";
var signedUri = uriString.ToSignedUri(secret, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

signedUri.OriginalString.ValidateSignedUri(secret);

Console.WriteLine(signedUri);

try
{
    var tampered = new UriBuilder(signedUri);
    tampered.Query = tampered.Query.TrimStart('?') + "&tampered=1";
    tampered.Uri.OriginalString.ValidateSignedUri(secret);
}
catch (SecurityException ex)
{
    Console.WriteLine(ex.Message);
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d1d5f02230e88bcf5abf2552"><a href="~/run-actions/d1d5f02230e88bcf5abf2552/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d1d5f02230e88bcf5abf2552/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
