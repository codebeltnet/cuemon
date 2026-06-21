---
uid: Cuemon.Reflection.ActivatorOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ActivatorOptions"/> with <see cref="ActivatorFactory.CreateInstance{T,TInstance}"/> to customize object creation with specific binding flags.

```csharp
using System;
using System.Reflection;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class ActivatorOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of ActivatorOptions
        var options = new ActivatorOptions
        {
            Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance
        };

        // Create a Uri instance using a factory with explicit binding flags
        var uri = ActivatorFactory.CreateInstance<string, Uri>("http://example.com", o =>
        {
            o.Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
        });

        Console.WriteLine(uri.Host);

}
}

```
