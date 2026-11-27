---
uid: Cuemon.Extensions.Reflection.AssemblyExtensions
example:
- *content
---

`AssemblyExtensions` provides extension methods for `Assembly` to retrieve version information and debug-build status. This example obtains the entry assembly and calls `GetAssemblyVersion`, `GetFileVersion`, and `GetProductVersion` to read version attributes, then checks `HasAlphanumericVersion` and `IsSemanticVersion()` on the returned `SourceVersion` objects. It also calls `IsDebugBuild()` to determine whether the assembly was compiled in Debug configuration. Console output displays each version string (e.g., `1.0.0.0`), whether it has alphanumeric or semantic version characteristics, and the debug-build flag.

```csharp
// Program.cs
using System;
using Assembly = System.Reflection.Assembly;
using Cuemon.Extensions.Reflection;

var assembly = typeof(AssemblyExtensions).Assembly;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7cf0d489e4dae6981802f960"><a href="~/run-actions/7cf0d489e4dae6981802f960/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7cf0d489e4dae6981802f960/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
