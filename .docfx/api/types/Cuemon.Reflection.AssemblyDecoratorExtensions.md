---
uid: Cuemon.Reflection.AssemblyDecoratorExtensions
example:
- *content
---

`AssemblyDecoratorExtensions` provides extension methods on `Decorator.Enclose` for inspecting assembly metadata, loading embedded resources, and filtering types. This example wraps the entry assembly and calls `IsDebugBuild`, `GetAssemblyVersion`, `GetFileVersion`, and `GetProductVersion` to retrieve version information. It also demonstrates `GetTypes` with optional namespace and interface filters, and `GetManifestResources` with various match modes including `ContainsName`, `Extension`, and `Name`. Console output displays each version string, boolean flags, and resource character counts.

```csharp
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        // Get the entry assembly to inspect
        var assembly = Assembly.GetExecutingAssembly();
        var decorator = Decorator.Enclose(assembly);

        // Check if it's a debug build
        bool isDebug = decorator.IsDebugBuild(); // true in Debug config, false in Release

        // Get version information
        var asmVersion = decorator.GetAssemblyVersion();        // from AssemblyVersionAttribute
        var fileVersion = decorator.GetFileVersion();            // from AssemblyFileVersionAttribute
        var productVersion = decorator.GetProductVersion();      // from AssemblyInformationalVersionAttribute

        Console.WriteLine($"Assembly version: {asmVersion}");
        Console.WriteLine($"File version: {fileVersion}");
        Console.WriteLine($"Product version: {productVersion}");

        // Get types from the assembly, optionally filtered
        var allTypes = decorator.GetTypes();                                    // all types in the assembly
        var filteredByNamespace = decorator.GetTypes(namespaceFilter: "Cuemon.Reflection"); // types in a specific namespace
        var filteredByInterface = decorator.GetTypes(typeFilter: typeof(IDisposable)); // types implementing IDisposable

        // Load embedded manifest resources (partial name match)
        var resources = decorator.GetManifestResources("config", ManifestResourceMatch.ContainsName);
        foreach (var resource in resources)
        {
    using var reader = new StreamReader(resource.Value);
            string content = reader.ReadToEnd();
            Console.WriteLine($"Resource '{resource.Key}': {content.Length} chars");

        // Find resources by file extension
        var jsonFiles = decorator.GetManifestResources(".json", ManifestResourceMatch.Extension);

        // Get a resource by exact name
        var singleResource = decorator.GetManifestResources("MyApp.Resources.data.xml", ManifestResourceMatch.Name);

}}
}

```
