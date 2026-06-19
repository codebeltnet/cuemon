---
uid: Cuemon.Extensions.Reflection.AssemblyExtensions
example:
- *content
---

The following example demonstrates how to retrieve assembly version, file version, and product version information from an assembly using AssemblyExtensions.

```csharp
using System;
using System.Reflection;
using Cuemon.Extensions.Reflection;

namespace MyApp.Reflection
{
    public class AssemblyExtensionsExample
    {
        public void Demonstrate()
        {
            var assembly = typeof(AssemblyExtensionsExample).Assembly;

            // Get the assembly version (from AssemblyVersionAttribute)
            var assemblyVersion = assembly.GetAssemblyVersion();
            Console.WriteLine($"Assembly version: {assemblyVersion}"); // e.g., "1.0.0.0"
            Console.WriteLine($"Has alphanumeric version: {assemblyVersion.HasAlphanumericVersion}"); // False
            Console.WriteLine($"Is semantic version: {assemblyVersion.IsSemanticVersion()}"); // False

            // Get the file version (from AssemblyFileVersionAttribute)
            var fileVersion = assembly.GetFileVersion();
            Console.WriteLine($"File version: {fileVersion}"); // e.g., "1.0.0.0"

            // Get the product version (from AssemblyInformationalVersionAttribute)
            var productVersion = assembly.GetProductVersion();
            Console.WriteLine($"Product version: {productVersion}"); // e.g., "1.0.0"
            Console.WriteLine($"Has alphanumeric version: {productVersion.HasAlphanumericVersion}"); // True (usually)
            Console.WriteLine($"Is semantic version: {productVersion.IsSemanticVersion()}"); // True

            // Check if the assembly is a debug build
            var isDebug = assembly.IsDebugBuild();
            Console.WriteLine($"Is debug build: {isDebug}"); // True in Debug, False in Release

}}
}

```
