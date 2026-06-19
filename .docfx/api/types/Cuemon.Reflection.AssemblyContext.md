---
uid: Cuemon.Reflection.AssemblyContext
example:
- *content
---

```csharp
using System;
using System.Linq;
using Cuemon.Reflection;

namespace Cuemon.Reflection;

public class AssemblyContextExample
{
    public void Demonstrate()
    {
        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = a => a.FullName?.StartsWith("Cuemon") == true;
        });

        foreach (var assembly in assemblies)
        {
            Console.WriteLine($"Assembly: {assembly.GetName().Name}");
        }
    }
}
```
