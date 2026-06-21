---
uid: Cuemon.Reflection.ParameterSignature
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.ParameterSignature"/> to extract parameter information from a method.

```csharp
using System;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class ParameterSignatureExample
{
    public void Demonstrate()
    {
        MethodInfo method = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });

        if (method != null)
        {
            var signatures = ParameterSignature.Parse(method).ToList();

            foreach (var signature in signatures)
            {
                Console.WriteLine($"Parameter: {signature.ParameterName}, Type: {signature.ParameterType.Name}");

}}}
}

```
