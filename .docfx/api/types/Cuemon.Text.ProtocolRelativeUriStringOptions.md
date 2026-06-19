---
uid: Cuemon.Text.ProtocolRelativeUriStringOptions
example:
- *content
---

The following example demonstrates how to configure ProtocolRelativeUriStringOptions to resolve protocol-relative URIs (such as //example.com/resource) by specifying the default protocol scheme.

```csharp
using System;
using Cuemon;
using Cuemon.Text;

namespace MyApp.Examples
{
    public class ProtocolRelativeUriStringOptionsExample
    {
        public void Demonstrate()
        {
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

}}
}

```
