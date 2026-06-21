---
uid: Cuemon.Reflection.VersionResult
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Reflection.VersionResult"/> to work with both numerical and semantic (alphanumeric) version strings.

```csharp
using System;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class VersionResultExample
{
    public void Demonstrate()
    {
        // Numerical version (parsable to System.Version)
        var numeric = new VersionResult("1.2.3.4");
        Console.WriteLine($"Value: {numeric.Value}");
        Console.WriteLine($"HasAlphanumericVersion: {numeric.HasAlphanumericVersion}");
        Console.WriteLine($"IsSemanticVersion: {numeric.IsSemanticVersion()}");

        // Semantic/alphanumeric version (not a pure numerical version)
        var semantic = new VersionResult("2.0.0-beta.1");
        Console.WriteLine($"\nValue: {semantic.Value}");
        Console.WriteLine($"HasAlphanumericVersion: {semantic.HasAlphanumericVersion}");
        Console.WriteLine($"IsSemanticVersion: {semantic.IsSemanticVersion()}");

        // Static helper check
        var check = VersionResult.IsSemanticVersion("3.0.0-rc.1");
        Console.WriteLine($"\nIs '3.0.0-rc.1' semantic? {check}");

}
}

```
