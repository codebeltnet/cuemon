---
uid: Cuemon.Reflection.ManifestResourceMatch
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ManifestResourceMatch"/> to specify how to locate embedded assembly resources.

```csharp
using System;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class ManifestResourceMatchExample
{
    public void Demonstrate()
    {
        var match = ManifestResourceMatch.Extension;

        switch (match)
        {
            case ManifestResourceMatch.Name:
                Console.WriteLine("Match by exact resource name.");
                break;
            case ManifestResourceMatch.ContainsName:
                Console.WriteLine("Match by partial name match.");
                break;
            case ManifestResourceMatch.Extension:
                Console.WriteLine("Match by file extension (e.g., .json).");
                break;
            case ManifestResourceMatch.ContainsExtension:
                Console.WriteLine("Match by partial extension match.");
                break;

}}
}

```
