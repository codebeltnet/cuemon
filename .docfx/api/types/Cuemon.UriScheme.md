---
uid: Cuemon.UriScheme
example:
- *content
---

The following example demonstrates how to use `UriScheme` with `StringFactory.CreateUriScheme` to generate URI scheme strings.

```csharp
using Cuemon;
using System;

namespace MyApp.Examples;

public class UriSchemeExample
{
    public void Demonstrate()
    {
        var httpsScheme = StringFactory.CreateUriScheme(UriScheme.Https);
        var ftpScheme = StringFactory.CreateUriScheme(UriScheme.Ftp);

        Console.WriteLine(httpsScheme); // outputs: https://
        Console.WriteLine(ftpScheme);   // outputs: ftp://

}
}

```
