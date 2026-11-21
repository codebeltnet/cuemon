---
uid: Cuemon.Text.ProtocolRelativeUriStringOptions
example:
- *content
---

The following example demonstrates how to configure ProtocolRelativeUriStringOptions to resolve protocol-relative URIs (such as //example.com/resource) by specifying the default protocol scheme.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Text;

// Configure how a protocol-relative URI (e.g., "//example.com/resource")
// is resolved to an absolute URI by specifying the default protocol.
var options = new ProtocolRelativeUriStringOptions
{
    Protocol = UriScheme.Https,
    RelativeReference = Alphanumeric.NetworkPathReference
};

Console.WriteLine($"Protocol: {options.Protocol}");
Console.WriteLine($"Relative reference: {options.RelativeReference}");

// Switch to HTTP for local development
options.Protocol = UriScheme.Http;
Console.WriteLine($"Updated protocol: {options.Protocol}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="22a83c612018cfd3ed666dc7"><a href="~/run-actions/22a83c612018cfd3ed666dc7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/22a83c612018cfd3ed666dc7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
