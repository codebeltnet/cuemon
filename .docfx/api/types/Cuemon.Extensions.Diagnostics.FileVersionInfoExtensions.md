---
uid: Cuemon.Extensions.Diagnostics.FileVersionInfoExtensions
example:
- *content
---

`FileVersionInfoExtensions` provides extension methods for `FileVersionInfo` to extract structured version information as `VersionResult` objects from an assembly's metadata. This example obtains the `FileVersionInfo` for the current assembly, then calls `ToProductVersion` to retrieve the NuGet/semantic version string and `ToFileVersion` to retrieve the file version string. Key steps include checking `IsSemanticVersion()` and `HasAlphanumericVersion` on the product version, and converting the file version to a `System.Version` via `ToVersion()`. Console output displays the assembly full name, original version strings, structured version results, and boolean flags for version characteristics.

```csharp
using System;
using System.Diagnostics;
using System.Reflection;
using Cuemon.Extensions.Diagnostics;
using Cuemon.Reflection;

namespace MyApp.Diagnostics
{
    public static class FileVersionInfoExtensionsExamples
    {
        public static void Demonstrate()
        {
            // Get the FileVersionInfo for the current assembly.
            Assembly assembly = typeof(FileVersionInfoExtensionsExamples).Assembly;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine("Assembly: {0}", assembly.FullName);
            Console.WriteLine("Original ProductVersion: {0}", fvi.ProductVersion);
            Console.WriteLine("Original FileVersion: {0}", fvi.FileVersion);

            // Convert to a structured VersionResult using the extension methods.

            // ToProductVersion returns the NuGet/semantic version string (e.g., "1.2.3-beta").
            VersionResult productVersion = fvi.ToProductVersion();
            Console.WriteLine("Product VersionResult: {0}", productVersion.Value);
            Console.WriteLine("Is semantic version? {0}", productVersion.IsSemanticVersion());
            Console.WriteLine("Has alphanumeric part? {0}", productVersion.HasAlphanumericVersion);

            // ToFileVersion returns the file version string (e.g., "1.2.3.0").
            VersionResult fileVersion = fvi.ToFileVersion();
            Console.WriteLine("File VersionResult: {0}", fileVersion.Value);

            // Both VersionResult objects can be converted to System.Version.
            Version numericVersion = fileVersion.ToVersion();
            Console.WriteLine("Parsed as System.Version: {0}", numericVersion);

}}
}

```
