---
uid: Cuemon.Extensions.Data.Integrity.AssemblyExtensions
example:
- *content
---

The following example demonstrates generating a <see cref="CacheValidator"/> from an assembly using the [GetCacheValidator](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.Data.Integrity.AssemblyExtensions.html#Cuemon_Extensions_Data_Integrity_AssemblyExtensions_GetCacheValidator_System_Reflection_Assembly_System_Func_Cuemon_Security_Hash__System_Action_Cuemon_Data_Integrity_FileChecksumOptions__) extension method.

```csharp
using System;
using System.Reflection;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Data.Integrity;

namespace MyApp.Examples;

public class AssemblyExtensionsExample
{
    public static void Main()
    {
        // Get a reference to any loaded assembly
        Assembly assembly = typeof(AssemblyExtensionsExample).Assembly;

        // Generate a CacheValidator from the assembly's file metadata
        CacheValidator validator = assembly.GetCacheValidator();

        Console.WriteLine($"Assembly: {assembly.GetName().Name}");
        Console.WriteLine($"Created (UTC): {validator.Created:O}");
        Console.WriteLine($"Modified (UTC): {validator.Modified?.ToString("O") ?? "N/A"}");
        Console.WriteLine($"Checksum (hex): {validator.Checksum.ToHexadecimalString()}");
        Console.WriteLine($"Validation strength: {validator.Validation}");

        // Use the CacheValidator for HTTP cache validation scenarios
        Console.WriteLine($"\nETag candidate: \"{validator.Checksum.ToHexadecimalString()}\"");

}
}

```
